using System.Collections.Generic;
using Abp;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;

namespace LyuAdmin.Settings
{
    public class EmailSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(EmailSettingNames.Smtp.Host, "smtp.qq.com", L("SmtpHost"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Port, "25", L("SmtpPort"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.UserName, "570678569", L("Username"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Password, "q284655", L("Password"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.Domain, null, L("DomainName"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                      // new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false", L("UseSSL"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "false", L("UseDefaultCredentials"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.DefaultFromAddress, "570678569@qq.com", L("DefaultFromSenderEmailAddress"), scopes: SettingScopes.Application | SettingScopes.Tenant),
                       new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "570678569@qq.com", L("DefaultFromSenderDisplayName"), scopes: SettingScopes.Application | SettingScopes.Tenant)
                   };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}