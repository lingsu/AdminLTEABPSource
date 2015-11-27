using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Lyu.Utility.Application.Services.Dto;
using LyuAdmin.Authorization.Roles;

namespace LyuAdmin.Roles.Dto
{
    public class GetRoleQueryInput : QueryRequestInput    {
        //DOTO:在这里增加查询参数

        /// <summary>
        /// 角色管理分类
        /// </summary>
        //public int? RoleCategoryId { get; set; }
    }
}

