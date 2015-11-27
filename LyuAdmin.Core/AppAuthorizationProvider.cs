using Abp.Authorization;
using Abp.Localization;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Users;

namespace LyuAdmin
{
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.CreatePermission(Permissions.Pages, new LocalizableString("Pages", LyuAdminConsts.PermissionsSourceName));
            var module = pages.CreateChildPermission(Permissions.Pages_Administration, new LocalizableString("PagesAdministration", LyuAdminConsts.PermissionsSourceName));

            var role = module.CreateChildPermission(RolesPermissions.Role, new LocalizableString("Role", LyuAdminConsts.PermissionsSourceName));
            role.CreateChildPermission(RolesPermissions.Role_CreateRole, new LocalizableString("CreateRole", LyuAdminConsts.PermissionsSourceName));
            role.CreateChildPermission(RolesPermissions.Role_UpdateRole, new LocalizableString("UpdateRole", LyuAdminConsts.PermissionsSourceName));
            role.CreateChildPermission(RolesPermissions.Role_DeleteRole, new LocalizableString("DeleteRole", LyuAdminConsts.PermissionsSourceName));

            //用户管理管理
            var user = module.CreateChildPermission(UsersPermissions.User, new LocalizableString("User", LyuAdminConsts.PermissionsSourceName));
            user.CreateChildPermission(UsersPermissions.User_CreateUser, new LocalizableString("CreateUser", LyuAdminConsts.PermissionsSourceName));
            user.CreateChildPermission(UsersPermissions.User_UpdateUser, new LocalizableString("UpdateUser", LyuAdminConsts.PermissionsSourceName));
            user.CreateChildPermission(UsersPermissions.User_DeleteUser, new LocalizableString("DeleteUser", LyuAdminConsts.PermissionsSourceName));

        }
    }
}