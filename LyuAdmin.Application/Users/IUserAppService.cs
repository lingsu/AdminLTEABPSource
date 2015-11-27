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

        #region �û�����

        /// <summary>
        /// ���ݲ�ѯ������ȡ�û���ҳ�б�
        /// </summary>
        Task<QueryResultOutput<UserQueryDto>> GetUserQuery(GetUserQueryInput input);


        /// <summary>
        /// ��ȡָ��id���û���Ϣ
        /// </summary>
        Task<UserDto> GetUser(long id);


        /// <summary>
        /// ����������û�
        /// </summary>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// �����û�
        /// </summary>
        Task CreateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// �����û�
        /// </summary>
        Task UpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// ɾ���û�
        /// </summary>
        Task DeleteUser(EntityRequestInput<long> input);

        /// <summary>
        /// ����ɾ���û�
        /// </summary>
        Task BatchDeleteUser(IEnumerable<int> input);


        #endregion
    }
}