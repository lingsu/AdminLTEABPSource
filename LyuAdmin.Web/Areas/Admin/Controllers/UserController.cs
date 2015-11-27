using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization;
using LyuAdmin.Roles;
using LyuAdmin.Users;
using LyuAdmin.Users.Dto;
using LyuAdmin.Web.Areas.Admin.Models.Users;

namespace LyuAdmin.Web.Areas.Admin.Controllers
{
    public class UserController : AdminControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        public UserController(IUserAppService userAppService, IRoleAppService roleAppService)
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Edit(long? id)
        {
            var model = new CreateOrUpdateViewModel();
          
            if (id.HasValue)
                model.User = await _userAppService.GetUser(id.Value);
            else
                model.User = new UserDto();

            model.Roles = await _roleAppService.GetRoleList();
           // ViewBag.roles = new SelectItem
            return PartialView(model);
        }
    }
}