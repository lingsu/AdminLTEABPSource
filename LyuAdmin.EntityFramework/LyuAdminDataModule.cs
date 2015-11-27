using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using LyuAdmin.EntityFramework;

namespace LyuAdmin
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(LyuAdminCoreModule))]
    public class LyuAdminDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
