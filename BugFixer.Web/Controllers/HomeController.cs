using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Questions;
using BugFixer.Application.ViewModels.User;
using BugFixer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BugFixer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,
            IQuestionService questionService,
            IUserService userService
            )
        {
            _logger = logger;
            _questionService = questionService;
            _userService = userService;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<QuestionVM> questionList = await _questionService.GetQuestionsServiceAsync();
            return View(questionList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        #region MyRegion
        [HttpGet("users-page")]
        public async Task<IActionResult> UsersPage(string OrderType,bool Reverse, int pageId = 1)
        {
            FilterUsersPageVM filterUsersPage=new FilterUsersPageVM()
            {
                Page=pageId,
                OrderType=OrderType,
                Reverse=Reverse
            };
            FilterUsersPageVM usersPage=await _userService.FilterUsersPageServiceAsync(filterUsersPage); 
            return View(usersPage);
        }
   

        #endregion
    }
}