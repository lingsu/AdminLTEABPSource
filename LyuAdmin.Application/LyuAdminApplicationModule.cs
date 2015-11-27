using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using AutoMapper;
using LyuAdmin.Users;
using LyuAdmin.Users.Dto;

namespace LyuAdmin
{
    [DependsOn(typeof(LyuAdminCoreModule), typeof(AbpAutoMapperModule))]
    public class LyuAdminApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
