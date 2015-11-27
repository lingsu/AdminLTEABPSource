using Abp.Application.Features;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.MultiTenancy;
using LyuAdmin.Users;

namespace LyuAdmin.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}