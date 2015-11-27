using Abp.Application.Editions;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Editions;
using LyuAdmin.Users;

namespace LyuAdmin.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        
        public TenantManager(IRepository<Tenant> tenantRepository, IRepository<TenantFeatureSetting, long> tenantFeatureRepository, EditionManager editionManager) : 
            base(tenantRepository, tenantFeatureRepository, editionManager)
        {
        }
    }
}