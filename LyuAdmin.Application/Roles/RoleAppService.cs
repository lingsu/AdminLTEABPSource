using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Roles.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using Lyu.Utility.Extensions;

namespace LyuAdmin.Roles
{
    /* THIS IS JUST A SAMPLE. */
    public class RoleAppService : LyuAdminAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        /// <summary>
        /// 获取所有<#= FunctionName #>列表
        /// </summary>
        public async Task<IEnumerable<RoleDto>> GetRoleList()
        {
            var query = _roleManager.Roles.OrderBy(x => x.CreationTime);
            var list = await query.To<RoleDto>().Take(100).ToListAsync();
            return list;
        }
    }
}