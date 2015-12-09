using System;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace LyuAdmin.Users
{
    public class UserStore : AbpUserStore<Tenant, Role, User>, IUserSecurityStampStore<User, long>
    {
        public UserStore(IRepository<User, long> userRepository, IRepository<UserLogin, long> userLoginRepository, IRepository<UserRole, long> userRoleRepository, IRepository<Role> roleRepository, IRepository<UserPermissionSetting, long> userPermissionSettingRepository, IUnitOfWorkManager unitOfWorkManager) : 
            base(userRepository, userLoginRepository, userRoleRepository, roleRepository, userPermissionSettingRepository, unitOfWorkManager)
        {
            
        }
        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            user.EmailConfirmationCode = stamp;
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult("");
        }
    }
}