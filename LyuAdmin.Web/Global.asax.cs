using System;
using Abp.Dependency;
using Abp.Web;
using Castle.Facilities.Logging;


namespace LyuAdmin.Web
{
    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //Bootstrap.Configure();
            IocManager.Instance.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);
        }
    }
}
