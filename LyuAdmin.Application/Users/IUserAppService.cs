using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Lyu.Utility.Application.Services.Dto;
using LyuAdmin.Users.Dto;

namespace LyuAdmin.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        #region 用户管理

        /// <summary>
        /// 根据查询条件获取用户分页列表
        /// </summary>
        Task<QueryResultOutput<UserQueryDto>> GetUserQuery(GetUserQueryInput input);


        /// <summary>
        /// 获取指定id的用户信息
        /// </summary>
        Task<UserDto> GetUser(long id);


        /// <summary>
        /// 新增或更改用户
        /// </summary>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// 新增用户
        /// </summary>
        Task CreateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task UpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// 删除用户
        /// </summary>
        Task DeleteUser(EntityRequestInput<long> input);

        /// <summary>
        /// 批量删除用户
        /// </summary>
        Task BatchDeleteUser(IEnumerable<int> input);


        #endregion
    }
}