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
        #region Properties
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public HomeController(ILogger<HomeController> logger,
    IQuestionService questionService,
    IUserService userService
    )
        {
            _logger = logger;
            _questionService = questionService;
            _userService = userService;
        }
        #endregion

        #region Index
        [HttpGet("/")]
        public async Task<IActionResult> Index(string orderType,int pageId=1)
        {
            FilterQuestionVM filterQuestionVM = new FilterQuestionVM()
            {
                Page=pageId,
                OrderType=orderType,
            };

            FilterQuestionVM result=await _questionService.FilterQuestionsAsync(filterQuestionVM);
            ViewBag.OrderType = orderType;
            return View(result);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





        #region Users Page
        [HttpGet("users-page")]
        public async Task<IActionResult> UsersPage(string OrderType,string userNameFilter, int pageId = 1)
        {
            ViewBag.UserNameFilter = userNameFilter;
            ViewBag.OrderTypeFilter = OrderType;
            FilterUsersPageVM filterUsersPage = new FilterUsersPageVM()
            {
                Page = pageId,
                OrderType = OrderType,
                UserName=userNameFilter
               
            };
            FilterUsersPageVM usersPage = await _userService.FilterUsersPageServiceAsync(filterUsersPage);
      
            return View(usersPage);
        }


        [HttpPost("userspageajax")]
        public async Task<IActionResult> UsersPageAjax(string OrderType, bool Reverse, int pageId = 1)
        {
            FilterUsersPageVM filterUsersPage = new FilterUsersPageVM()
            {
                Page = pageId,
                OrderType = OrderType,
                Reverse = Reverse
            };
            FilterUsersPageVM usersPage = await _userService.FilterUsersPageServiceAsync(filterUsersPage);
            return View(usersPage);
        }

        #endregion
    }
}