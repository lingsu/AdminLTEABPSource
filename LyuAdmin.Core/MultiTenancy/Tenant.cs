using Abp.MultiTenancy;
using LyuAdmin.Users;

namespace LyuAdmin.MultiTenancy
{
    public class Tenant : AbpTenant<Tenant, User>
    {
        public string Url { get; set; }
        public string Hosts { get; set; }
    }
}