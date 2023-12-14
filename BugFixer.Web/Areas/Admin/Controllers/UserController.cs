using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Role;
using BugFixer.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
 
        
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
         
            _roleService = roleService;
        }
        [HttpGet("admin/user-list")]
        public async Task<IActionResult> Index(string userNameFilter,int takeEntityFilter=10,int pageId=1)
        {
            FilterUsersViewModel filterUsersViewModel = new FilterUsersViewModel()
            {
                UserName = userNameFilter,
                TakeEntity=takeEntityFilter,
                Page=pageId
            };
            FilterUsersViewModel userLIst = await _userService.FilterUser(filterUsersViewModel);
            ViewBag.usernameFilter = userNameFilter;
            ViewBag.takeEntityFilter = takeEntityFilter;
            return View(userLIst);
        }
        [HttpGet]
        public async Task<IActionResult> FilterUsersAjax(FilterUsersViewModel filterUsers)
        {
            return RedirectToAction("Index", filterUsers);
        }

        [HttpGet("admin/create-user")]
        public  async Task<IActionResult> Create()
        {
            IEnumerable<RoleVM> roleList =await _roleService.GetAllServiceAsync();
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost("admin/create-user")]
        public async Task<IActionResult> Create(CreateUserVM createUser,int selectedRole)
        {
            if (!ModelState.IsValid)
            {
                return View(createUser);
            }
            createUser.RoleId= selectedRole;
            await _userService.CreateServiceAsync(createUser);
            return RedirectToAction("Index");
        }


        [HttpGet("admin/update-user/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            IEnumerable<RoleVM> roleList = await _roleService.GetAllServiceAsync();
            ViewBag.RoleList = roleList;
            UpdateUserVM userInfor = await _userService.GetUserInforForUpdate(id);
            return View(userInfor);
        }


        [HttpPost("admin/update-user/{id}")]
        public async Task<IActionResult> Update(UpdateUserVM updateUserVM, int selectedRole)
        {

            if (!ModelState.IsValid)
            {
                return View(updateUserVM);
            }
            updateUserVM.RoleId= selectedRole;
            await _userService.UpdateServiceAsync(updateUserVM);
            return RedirectToAction("Index");
        }


        [HttpGet("admin/show-user/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            UserVM userInfor = await _userService.GetServiceAsync(id);
            return View(userInfor);
        }


        [HttpGet("admin/delete-user/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteServiceAsync(id);
            return Json(new { status = "success" });
        }



    }
}
