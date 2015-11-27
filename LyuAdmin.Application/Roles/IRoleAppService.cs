using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using LyuAdmin.Roles.Dto;

namespace LyuAdmin.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
        Task<IEnumerable<RoleDto>> GetRoleList();
    }
}
