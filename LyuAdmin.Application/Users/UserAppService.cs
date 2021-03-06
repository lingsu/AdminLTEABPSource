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
        private readonly MessageManager _messageManager;

        public UserAppService(UserManager userManager, RoleManager roleManager, IPermissionManager permissionManager, IRepository<UserRole, long> userRoleRepository, ISettingManager settingManager, IEmailSender emailSender, IMessageTokenProvider messageTokenProvider, ITokenizer tokenizer, MessageManager messageManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _userRoleRepository = userRoleRepository;
            _settingManager = settingManager;
            _emailSender = emailSender;
            _messageTokenProvider = messageTokenProvider;
            _tokenizer = tokenizer;
            _messageManager = messageManager;
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

        #region 用户管理

        /// <summary>
        /// 根据查询条件获取用户分页列表
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
        /// 获取指定id的用户信息
        /// </summary>
        public async Task<UserDto> GetUser(long id)
        {
            var entity = await _userManager.GetUserByIdAsync(id);
            var dto = entity.MapTo<UserDto>();
            dto.AssignedRoleNames = await _userManager.GetRolesAsync(id);
            return dto;
        }

        /// <summary>
        /// 新增或更改用户
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
        /// 新增用户
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

        private async Task<int> SendActivationEmail(User user)
        {
            var messageTemplate = await _messageManager.GetActiveMessageTemplate("Customer.EmailValidationMessage");
            if (messageTemplate == null)
                return 0;

            var tokens = new List<Token>();
            _messageTokenProvider.AddUserTokens(tokens, user);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            _emailSender.Send("q25a25q@live.com", _tokenizer.Replace(messageTemplate.Subject, tokens,false), _tokenizer.Replace(messageTemplate.Body, tokens, true));
            //entity.EmailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(entity.Id);
            //await _userManager.SendEmailAsync(entity.Id, "sss", "bbb");

            return 1;
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_UpdateUser)]
        public virtual async Task UpdateUser(CreateOrUpdateUserInput input)
        {
            var entity = await _userManager.GetUserByIdAsync(input.User.Id);

            if (IsAdminUser(entity) && input.User.UserName != entity.UserName)
            {
                throw new UserFriendlyException("管理用户，不能修改用户名");
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
            //var sff = await _userManager.UpdateSecurityStampAsync(entity.Id);
            var b = await UserManager.GenerateUserTokenAsync("EmailConfirmation", entity.Id);
            string code = UserManager.GeneratePasswordResetToken(entity.Id);
            var s = await _userManager.GenerateEmailConfirmationTokenAsync(entity.Id);
            if (input.SendActivationEmail)
            {
               await SendActivationEmail(entity);
            }
        }

      
        /// <summary>
        /// 删除用户
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_DeleteUser)]
        public async Task DeleteUser(EntityRequestInput<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var user = await _userManager.GetUserByIdAsync(input.Id);
            if (IsAdminUser(user))
            {
                throw new UserFriendlyException("管理用户，不能删除！");
            }

            await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        [AbpAuthorize(UsersPermissions.User_DeleteUser)]
        public async Task BatchDeleteUser(IEnumerable<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
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