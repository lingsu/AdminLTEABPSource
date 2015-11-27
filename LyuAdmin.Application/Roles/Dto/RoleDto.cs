
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Abp.Extensions;
using LyuAdmin.Authorization.Roles;


namespace LyuAdmin.Roles.Dto
{

    /// <summary>
    /// 角色管理
    /// </summary>

    [AutoMap(typeof(Role))]
    public class RoleDto : EntityDto, IValidate
    {
        /// <summary>
        /// string
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// string
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// bool
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// bool
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// bool
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}


