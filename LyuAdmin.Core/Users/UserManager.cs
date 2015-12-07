using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace LyuAdmin.Users
{
    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(
            UserStore store,
            RoleManager roleManager,
            EmailService emailService,
            IRepository<Tenant> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            ICacheManager cacheManager)
            : base(
                store,
                roleManager,
                tenantRepository,
                multiTenancyConfig,
                permissionManager,
                unitOfWorkManager,
                settingManager,
                userManagementConfig,
                iocResolver,
                cacheManager
            )
        {
            this.EmailService = emailService;

            //RegisterTwoFactorProvider("PhoneCode",new PhoneNumberTokenProvider<User,long>()
            //{
            //    MessageFormat = "your code is {0}"
            //});

            //RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User, long>()
            //{
            //    Subject = "securiyCode",
            //    BodyFormat = "your code is {0}"
            //});
            UserTokenProvider = new EmailTokenProvider<User, long>()
            {
                Subject = "securiyCode",
                BodyFormat = "your code is {0}"
            };
            //IUserSecurityStampStore<>
            //UserTokenProvider =new DataProtectorTokenProvider
        }
    }
}