using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;
using Abp.Localization.Sources.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using LyuAdmin.Users;

namespace LyuAdmin
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class LyuAdminCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Remove the following line to disable multi-tenancy.
            Configuration.MultiTenancy.IsEnabled = true;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    LyuAdminConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "LyuAdmin.Localization.Source"
                        )
                    )
                );

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    LyuAdminConsts.PermissionsSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "LyuAdmin.Localization.Permissions"
                        )
                    )
                );

           // Configuration.
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Settings.Providers.Add<UserSettingProvider>();
            Configuration.Navigation.Providers.Add<AdministrationNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
