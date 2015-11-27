using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Lyu.Utility.Application.Services.Dto;
using LyuAdmin.Users;

namespace LyuAdmin.Users.Dto
{
    public class GetUserQueryInput : QueryRequestInput    {
        //DOTO:在这里增加查询参数

        /// <summary>
        /// 用户分类
        /// </summary>
        //public int? UserCategoryId { get; set; }
    }
}

