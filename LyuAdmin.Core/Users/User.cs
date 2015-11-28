using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using LyuAdmin.MultiTenancy;

namespace LyuAdmin.Users
{
    [Description("用户")]
    public class User : AbpUser<Tenant, User>
    {
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
    }
}