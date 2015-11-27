using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Extensions;
using LyuAdmin.Authorization.Roles;

namespace LyuAdmin.Roles.Dto
{
    [AutoMapFrom(typeof(Role))]
    public class RoleQueryDto : EntityDto
    {
        /// <summary>
        /// string
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// string
        /// </summary>
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

