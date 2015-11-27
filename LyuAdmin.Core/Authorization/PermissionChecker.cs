using Abp.Authorization;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using LyuAdmin.Users;

namespace LyuAdmin.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
