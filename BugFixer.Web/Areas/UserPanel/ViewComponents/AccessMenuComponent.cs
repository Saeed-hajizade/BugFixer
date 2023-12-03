using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugFixer.Web.Areas.UserPanel.ViewComponents
{
    public class AccessMenuComponent:ViewComponent
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        public AccessMenuComponent(IQuestionService questionService, IUserService userService)
        {
            _questionService = questionService;
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var qaCounts = await _questionService.GetQACountsServiceAsync(userId);
            var followersFollowingsCount = await _userService.GetUserFollowingsFollowersCountServiceAsync(userId);

            var viewModel = new AccessMenuVM
            {
                FollowersCount = followersFollowingsCount.FollowersCount,
                FollowingsCount = followersFollowingsCount.FollowingsCount,
                QuestionsCount = qaCounts.QuestionsCount,
                AnswersCount = qaCounts.AnswersCount,
            };
            
            return View(viewModel);
        }
    }
}
