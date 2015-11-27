using Abp.Application.Navigation;
using Abp.Localization;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Users;

namespace LyuAdmin
{
    public class AdministrationNavigationProvider: NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var administrationMenu = new MenuDefinition("AdministrationMenu", new FixedLocalizableString("后台管理"));

            var pagees = new MenuItemDefinition(
                Permissions.Pages_Administration,
                new LocalizableString("PagesAdministration",LyuAdminConsts.PermissionsSourceName),
                "fa fa-wrench",
                "/admin/dashboard",
                true,
                Permissions.Pages_Administration
                );

            var role = new MenuItemDefinition(
                RolesPermissions.Role,
                new LocalizableString("Role", LyuAdminConsts.PermissionsSourceName),
                "fa fa-circle-o",
                "/admin/role",
                true,
                RolesPermissions.Role
                );

            var user = new MenuItemDefinition(
               UsersPermissions.User,
               new LocalizableString("User", LyuAdminConsts.PermissionsSourceName),
               "fa fa-circle-o",
               "/admin/user",
               true,
               UsersPermissions.User
               );

            pagees.AddItem(user);
            pagees.AddItem(role);

            administrationMenu
              .AddItem(pagees);
            context.Manager.Menus.Add("AdministrationMenu", administrationMenu);
        }
    }
}