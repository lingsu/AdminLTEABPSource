using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Configuration;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using LyuAdmin.Api;
using LyuAdmin.Localization;

namespace LyuAdmin.Web
{
    [DependsOn(typeof(LyuAdminDataModule), typeof(LyuAdminApplicationModule), typeof(LyuAdminWebApiModule))]
    public class LyuAdminWebModule : AbpModule
    {

        public override void PreInitialize()
        {

            //Configuration.Localization.Sources.Add(
            //    new DictionaryBasedLocalizationSource(
            //        LyuAdminConsts.PermissionsSourceName,
            //        new XmlFileLocalizationDictionaryProvider(
            //            HttpContext.Current.Server.MapPath("~/Areas/Admin/Localization")
            //            )
            //        )
            //    );

            //Add/remove languages for your application
            // Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));
            // Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn", true));
            Configuration.Settings.Providers.Add<CnLocalizationSettingProvider>();
           // var e = Configuration.Modules.AbpConfiguration.Get<string>(LocalizationSettingNames.DefaultLanguage);
            //Configuration.Modules.AbpConfiguration.Set(LocalizationSettingNames.DefaultLanguage, "zh-CN");
            Configuration.Navigation.Providers.Add<LyuAdminNavigationProvider>();
        }

        public override void PostInitialize()
        {
            //IocManager.Resolve<ISettingDefinitionManager>().
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
