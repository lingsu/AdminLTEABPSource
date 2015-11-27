using System.Collections.Generic;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace LyuAdmin.Users.Dto
{
    public class CreateOrUpdateUserInput: IInputDto
    {
        public UserDto User { get; set; }
        public IEnumerable<string> AssignedRoleNames { get; set; }
        [DisplayName("使用随机密码")]
        public bool SendActivationEmail { get; set; }
    }
}