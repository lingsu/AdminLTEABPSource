using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using LyuAdmin.Authorization.Roles;
using LyuAdmin.Roles.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Lyu.Utility.Application.Services.Dto;
using Lyu.Utility.Extensions;
using Lyu.Utility.Application.Services.Dto.Extensions;

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

        #region 角色管理管理

        /// <summary>
        /// 根据查询条件获取角色管理分页列表
        /// </summary>
        public async Task<QueryResultOutput<RoleQueryDto>> GetRoleQuery(GetRoleQueryInput input)
        {
            var result = await _roleManager.Roles
                //TODO:根据传入的参数添加过滤条件
                //.WhereIf(input.RoleCategoryId.HasValue, m => m.RoleCategoryId == input.RoleCategoryId)
                //.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), m => m.Title.Contains(input.Keywords))
                .ToOutputAsync<RoleQueryDto>(input);
            return result;
        }

        /// <summary>
        /// 获取指定id的角色管理信息
        /// </summary>
        public async Task<RoleDto> GetRole(int id)
        {
            var entity = await _roleManager.GetRoleByIdAsync(id);
            return entity.MapTo<RoleDto>();
        }

        /// <summary>
        /// 新增或更改角色管理
        /// </summary>
        public async Task CreateOrUpdateRole(RoleDto input)
        {
            if (input.Id == 0)
            {
                await CreateRole(input);
            }
            else
            {
                await UpdateRole(input);
            }
        }

        /// <summary>
        /// 新增角色管理
        /// </summary>
        [AbpAuthorize(RolesPermissions.Role_CreateRole)]
        public virtual async Task CreateRole(RoleDto input)
        {
            //if (await _roleRepository.IsExistsRoleByName(input.CategoryName))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _roleManager.CreateAsync(input.MapTo<Role>());
           
        }

        /// <summary>
        /// 更新角色管理
        /// </summary>
        [AbpAuthorize(RolesPermissions.Role_UpdateRole)]
        public virtual async Task UpdateRole(RoleDto input)
        {
            //if (await _roleRepository.IsExistsRoleByName(input.CategoryName, input.Id))
            //{
            //    throw new UserFriendlyException(L("NameIsExists"));
            //}
            var entity = await _roleManager.GetRoleByIdAsync(input.Id);
            await _roleManager.UpdateAsync(input.MapTo(entity));
        }

        /// <summary>
        /// 删除角色管理
        /// </summary>
        [AbpAuthorize(RolesPermissions.Role_DeleteRole)]
        public async Task DeleteRole(EntityRequestInput input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var entity = await _roleManager.GetRoleByIdAsync(input.Id);
            await _roleManager.DeleteAsync(entity);
        }

        /// <summary>
        /// 批量删除角色管理
        /// </summary>
        [AbpAuthorize(RolesPermissions.Role_DeleteRole)]
        public async Task BatchDeleteRole(IEnumerable<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            //await _roleRepository.DeleteAsync(input);

        }

        #endregion
    }
}