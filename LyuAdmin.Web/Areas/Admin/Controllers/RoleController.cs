using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using LyuAdmin.Roles;
using LyuAdmin.Roles.Dto;

namespace LyuAdmin.Web.Areas.Admin.Controllers
{
    public class RoleController : AdminControllerBase
    {

        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
           
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Edit(int? id)
        {
            RoleDto model;
            if (!id.HasValue)  //新建
            {
                model = new RoleDto();
            }
            else  //编辑
            {
                model = await _roleAppService.GetRole(id.Value);
            }
            return View(model);
        }
    }
}
