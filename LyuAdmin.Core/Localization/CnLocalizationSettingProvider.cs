using System.Collections.Generic;
using Abp;
using Abp.Configuration;
using Abp.Localization;

namespace LyuAdmin.Localization
{
    public class CnLocalizationSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(LocalizationSettingNames.DefaultLanguage, "zh-CN", L("DefaultLanguage"), scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}