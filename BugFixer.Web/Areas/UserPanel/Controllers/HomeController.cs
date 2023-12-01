using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using System.Security.Claims;
using BugFixer.Domain.Models.Resume;
using BugFixer.Application.ViewModels.Resume;
using Newtonsoft.Json;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields
        private readonly IAccountService _accountService;
        private readonly IResumeService _resumeService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        #endregion


        #region Constructor
        public HomeController(IAccountService accountService, IResumeService resumeService, IQuestionService questionService, IAnswerService answerService)
        {
            _accountService = accountService;
            _resumeService = resumeService;
            _questionService = questionService;
            _answerService = answerService; 
        }
        #endregion


        #region Actions 

        #region Index
        [Route("/user-panel")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Edit

        [Route("/user-panel/edit")]
        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _accountService.GetUserForEditByIdServiceAsync(userId);
            return View(user);
        }

        [HttpPost("/user-panel/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM edit)
        {
            if(!ModelState.IsValid)
                return View(edit);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _accountService.EditProfileByIdServiceAsync(userId, edit);
            ViewBag.isSuccess = true;
            return View();
        }

        #endregion

        #region ChangePassword
        [Route("/user-panel/change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost("/user-panel/change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM change)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
                return View(change);

            if (!await _accountService.ChangePasswordServiceAsync(userId, change))
                ModelState.AddModelError("OldPassword", "رمز عبور وارد شدده صحیح نمی باشد.");

            await _accountService.ChangePasswordServiceAsync(userId, change);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ChangePasswordSuccess"] = true;

            return Redirect("/login");
        }
        #endregion

        #region Resume
        [HttpGet("/user-panel/resume")]
        public IActionResult Resume()
        {
            return View();
        }

        [HttpPost("/user-panel/resume")]
        public async Task<IActionResult> Resume(string resumeWorkExperience, string resumebio,string resumeProfessionName, string resumeSkills, string resumeEducations, string resumeFavourites)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var resumeSkillsDs = JsonConvert.DeserializeObject<List<ResumeSkillsVM>>(resumeSkills);
            var resumeEducationsDs = JsonConvert.DeserializeObject<string>(resumeEducations);
            var resumeFavouritesDs = JsonConvert.DeserializeObject<string>(resumeFavourites);
            var resumebioDs = JsonConvert.DeserializeObject<string>(resumebio);
            var resumeWorkExperienceDs = JsonConvert.DeserializeObject<string>(resumeWorkExperience);
            var resumeProfessionNameDs = JsonConvert.DeserializeObject<string>(resumeProfessionName);

            var model = new CreateResumeVM{
                ProfessionName=resumeProfessionNameDs,
                WorkExperienceYears=resumeWorkExperienceDs,
                Bio=resumebioDs,
                ResumeSkills= resumeSkillsDs,
                EducationalDocuments=resumeEducationsDs,
                Favourites=resumeFavouritesDs,
                UserId=userId
            };
            await _resumeService.CreateResumeServiceAsync(model);
            return View();
        }
        #endregion

        #region Questions

        [Route("/user-panel/questions")]
        public async Task<IActionResult> Questions()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var questions = await _questionService.GetUserQuestionsSeviceAsync(userId);
            return View(questions);
        }

        #endregion

        #endregion
    }
}
