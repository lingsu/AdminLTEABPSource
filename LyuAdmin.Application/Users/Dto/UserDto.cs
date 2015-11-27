using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.Runtime.Validation;
using AutoMapper;
using Lyu.Utility.Mvc.ComponentModel.DataAnnotations;

namespace LyuAdmin.Users.Dto
{
    [AutoMap(typeof(User))]
    public class UserDto : Entity<long>, IInputDto, ICustomValidate
    {
        private IList<string> _assignedRoleNames;
        [Required]
        [DisplayName("名字")]
        public string Name { get; set; }
        [StringLength(32)]
        [Required]
        [DisplayName("姓氏")]
        public string Surname { get; set; }

        [StringLength(32)]
        [Required]
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [StringLength(32,MinimumLength = 6)]
        [DisplayName("密码")]
        //[IgnoreMap]
        public string Password { get; set; }
        [Compare("Password")]
        [StringLength(32, MinimumLength = 6)]
        [DisplayName("密码 (核对)")]
        public string PasswordRepeat { get; set; }
        [Required]
        [StringLength(256)]
        [DisplayName("邮箱地址")]
      
        //[EmailAddress]
        [Email]
        public string EmailAddress { get; set; }

        [DisplayName("下次登录需要修改密码")]
        public bool ShouldChangePasswordOnNextLogin { get; set; }
        [DisplayName("发送激活邮件")]
        public string SendActivationEmail { get; set; }
        [DisplayName("激活")]
        public bool IsActive { get; set; }
        [DisplayName("使用默认密码")]
        public bool SetRandomPassword { get; set; }

        public IList<string> AssignedRoleNames
        {
            get { return _assignedRoleNames ?? (_assignedRoleNames = new List<string>()); }
            set { _assignedRoleNames = value; } 
        }

        public void AddValidationErrors(List<ValidationResult> results)
        {
            if (Id == 0)
            {
                if (!SetRandomPassword && Password.IsNullOrEmpty())
                {
                    results.Add(new ValidationResult("这是必填字段", new List<string>() { "Password" }));
                }
            }
        }
    }
}