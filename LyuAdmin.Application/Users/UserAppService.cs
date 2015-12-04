using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Net.Mail;
using Abp.UI;
using Lyu.Abp.Core.Messages;
using Lyu.Utility.Application.Services.Dto;
using Lyu.Utility.Application.Services.Dto.Extensions;
using Lyu.Utility.Extensions;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Messages;
using LyuAdmin.Roles.Dto;
using LyuAdmin.Users.Dto;
using Microsoft.AspNet.Identity;

namespace LyuAdmin.Users
{
    /* THIS IS JUST A SAMPLE. */
    public class UserAppService : LyuAdminAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<UserRole,long> _userRoleRepository;
        private readonly ISettingManager _settingManager;
        private readonly IEmailSender _emailSender;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ITokenizer _tokenizer;

        public UserAppService(UserManager userManager, RoleManager roleManager, IPermissionManager permissionManager, IRepository<UserRole, long> userRoleRepository, ISettingManager settingManager, IEmailSender emailSender, IMessageTokenProvider messageTokenProvider, ITokenizer tokenizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _userRoleRepository = userRoleRepository;
            _settingManager = settingManager;
            _emailSender = emailSender;
            _messageTokenProvider = messageTokenProvider;
            _tokenizer = tokenizer;
        }


        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);
            //_userManager.Users.To<User>()
            await _userManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {

            CheckErrors(await _userManager.RemoveFromRoleAsync(userId, roleName));
        }

        #region �û�����

        /// <summary>
        /// ���ݲ�ѯ������ȡ�û���ҳ�б�
        /// </summary>
        public async Task<QueryResultOutput<UserQueryDto>> GetUserQuery(GetUserQueryInput input)
        {
            var result = await _userManager.Users
                .WhereIf(!input.Search.Value.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Search.Value))
                .OrderByDescending(x => x.CreationTime)
               .ToOutputAsync<UserQueryDto>(input);
            foreach (var userQueryDto in result.Data)
            {
                //var roles = await _userManager.GetRolesAsync(userQueryDto.Id);
                //userQueryDto.AssignedRoleNames = roles.MapTo<List<RoleQueryDto>>();
                userQueryDto.AssignedRoleNames = await (from userRole in _userRoleRepository.GetAll()
                                                        join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                                        where userRole.UserId == userQueryDto.Id
                                                        select role).To<RoleQueryDto>().ToListAsync();
            }

            return result;
        }
      

        /// <summary>
        /// ��ȡָ��id���û���Ϣ
        /// </summary>
        public async Task<UserDto> GetUser(long id)
        {
            var entity = await _userManager.GetUserByIdAsync(id);
            var dto = entity.MapTo<UserDto>();
            dto.AssignedRoleNames = await _userManager.GetRolesAsync(id);
            return dto;
        }

        /// <summary>
        /// ����������û�
        /// </summary>
        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
           
            if (input.User.Id == 0)
            {
                await CreateUser(input);
            }
            else
            {
                await UpdateUser(input);
            }
        }

        /// <summary>
        /// �����û�
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_CreateUser)]
        public  async Task CreateUser(CreateOrUpdateUserInput input)
        {
            var entity = input.User.MapTo<User>();
            
            entity.Roles = new List<UserRole>();
            foreach (var assignedRoleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(assignedRoleName);
                if (role != null)
                {
                    entity.Roles.Add(new UserRole { RoleId = role.Id });
                }
            }

            if (input.User.SetRandomPassword)
                input.User.Password = _settingManager.GetSettingValue(UserSettingNames.DefaultPassword);
            
            var identityResult = await _userManager.CreateAsync(entity, input.User.Password);
           
            identityResult.CheckErrors(LocalizationManager);

           
            //if (input.SendActivationEmail)
            //{
            //    //entity.EmailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(entity.Id);
            //    await _userManager.SendEmailAsync(entity.Id, "sss", "bbb");
            //}
        }

        /// <summary>
        /// �����û�
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_UpdateUser)]
        public virtual async Task UpdateUser(CreateOrUpdateUserInput input)
        {
            var entity = await _userManager.GetUserByIdAsync(input.User.Id);

            if (IsAdminUser(entity) && input.User.UserName != entity.UserName)
            {
                throw new UserFriendlyException("�����û��������޸��û���");
            }

            entity.UserName = input.User.UserName;
            entity.Name = input.User.Name;
            entity.Surname = input.User.Surname;
            entity.EmailAddress = input.User.EmailAddress;
            entity.IsActive = input.User.IsActive;

            //input.User.MapTo(entity);
            if (!string.IsNullOrEmpty(input.User.Password))
                entity.Password = _userManager.PasswordHasher.HashPassword(input.User.Password);
            //_userManager.ema
            var newCustomerRoles = new List<Role>();

            var allCustomerRoles = await _roleManager.Roles.ToListAsync();

            foreach (var assignedRoleName in allCustomerRoles)
                if (input.AssignedRoleNames.Contains(assignedRoleName.Name))
                    newCustomerRoles.Add(assignedRoleName);

            var customerRolesError = ValidateCustomerRoles(newCustomerRoles);

            foreach (var customerRole in allCustomerRoles)
            {
                if (input.AssignedRoleNames.Contains(customerRole.Name))
                {
                    if (entity.Roles.Any(x=>x.RoleId != customerRole.Id))
                    {
                        _userManager.AddToRole(entity.Id, customerRole.Name);
                       // entity.Roles.Add(new UserRole(entity.Id, customerRole.Id));
                    }
                }
                else
                {
                    var role = entity.Roles.FirstOrDefault(x => x.RoleId == customerRole.Id);
                    if (role != null)
                    {
                        entity.Roles.Remove(role);
                        _userManager.RemoveFromRole(entity.Id, customerRole.Name);
                    }
                }
            }
            // entity.Roles = newCustomerRoles;
            //foreach (var userRole in entity.Roles)
            //{
            //    userRole.RoleId
            //}

            var identityResult = await _userManager.UpdateAsync(entity);
            identityResult.CheckErrors(LocalizationManager);

            //tokens

            // _userManager.UserTokenProvider = new EmailTokenProvider<User, long>();
            //new DataProtectorTokenProvider(provider.Create("PasswordReset"));

            //await _userManager.EmailService.SendAsync(new IdentityMessage()
            //{
            //    Body = "bbb",
            //    Destination = entity.EmailAddress,
            //    Subject = "aaaa"
            //});


            if (input.SendActivationEmail)
            {
                var tokens = new List<Token>();
                _messageTokenProvider.AddUserTokens(tokens, entity);
                //_emailSender.Send("q25a25q@live.com", "subject", "bb");
                //entity.EmailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(entity.Id);
                //await _userManager.SendEmailAsync(entity.Id, "sss", "bbb");
            }
        }

      
        /// <summary>
        /// ɾ���û�
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_DeleteUser)]
        public async Task DeleteUser(EntityRequestInput<long> input)
        {
            //TODO:ɾ��ǰ���߼��жϣ��Ƿ�����ɾ��
            var user = await _userManager.GetUserByIdAsync(input.Id);
            if (IsAdminUser(user))
            {
                throw new UserFriendlyException("�����û�������ɾ����");
            }

            await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// ����ɾ���û�
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_DeleteUser)]
        public async Task BatchDeleteUser(IEnumerable<int> input)
        {
            //TODO:����ɾ��ǰ���߼��жϣ��Ƿ�����ɾ��
            foreach (var i in input)
            {
                var user = await _userManager.GetUserByIdAsync(i);
                await _userManager.DeleteAsync(user);
            }
        }

        #endregion


        private void SendAActiveEmail(User user)
        {
            //_emailSender.Send();
        }

        private string ValidateCustomerRoles(IList<Role> customerRoles)
        {

            //no errors
            return "";
        }

        private bool IsAdminUser(User user)
        {
            return user.UserName == _settingManager.GetSettingValue(UserSettingNames.DefaultAdminUserName);
        }
    }
}