using System.ComponentModel;
using Abp.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using LyuAdmin.Users;

namespace LyuAdmin.Authorization.Roles
{
    [Description("角色管理")]
    public class Role : AbpRole<Tenant, User>
    {

    }
}