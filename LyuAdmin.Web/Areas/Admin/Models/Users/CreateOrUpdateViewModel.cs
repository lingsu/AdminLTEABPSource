using System.Collections.Generic;
using LyuAdmin.Roles.Dto;
using LyuAdmin.Users.Dto;

namespace LyuAdmin.Web.Areas.Admin.Models.Users
{
    public class CreateOrUpdateViewModel
    {
         public UserDto User { get; set; }
         public IEnumerable<RoleDto> Roles { get; set; }
    }
}