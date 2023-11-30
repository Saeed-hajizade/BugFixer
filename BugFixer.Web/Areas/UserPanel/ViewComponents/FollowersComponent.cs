using BugFixer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugFixer.Web.Areas.UserPanel.ViewComponents
{
    public class FollowersComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public FollowersComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var followersFollowingsCount =await _userService.GetUserFollowingsFollowersCountServiceAsync(userId);
            return View(followersFollowingsCount);
        }
    }
}
