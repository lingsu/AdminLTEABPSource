using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace LyuAdmin.Users
{
    public class UserSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(UserSettingNames.DefaultAdminUserName, "admin", new FixedLocalizableString("默认管理者用户名"), scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
       
    }
}