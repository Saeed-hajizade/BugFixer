using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Account;
using BugFixer.Application.ViewModels.User;
using EShop.Application.Convertors;
using EShop.Application.Senders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BugFixer.Application.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace BugFixer.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IQuestionService _questionService;
        private IViewRenderService _viewRender;

        #endregion

        #region Constructor
        public AccountController(IAccountService accountService,
            IViewRenderService viewRender,
            IUserService userService,
            IQuestionService questionService
            )
        {
            _accountService = accountService;
            _viewRender = viewRender;
            _userService = userService;
            _questionService = questionService;
        }
        #endregion

        #region Actions


        #region Register
        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {

                return View(register);

            }

            if (await _accountService.IsEmailExistServiceAsync(register.Email) == true)
            {
                ModelState.AddModelError("Email", "حساب کاربری با این ایمیل موجود می باشد");
                return View(register);
            };

            if (await _accountService.IsUserNameExistServiceAsync(register.UserName) == true)
            {
                ModelState.AddModelError("UserName", "حساب کاربری از قبل موجود می باشد");
                return View(register);
            };

            UserVM user = await _accountService.RegisterServiceAsync(register);

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);

            return View("SuccessRegister", user);
        }
        #endregion

        #region Login

        [Route("login")]
        public IActionResult Login(bool IsActive = false)
        {
            ViewBag.IsActive = IsActive;
            return View();
        }


        [HttpPost("login")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
                return View();

            UserVM user = await _accountService.LoginUserServiceAsync(login);
            if (user != null)
            {
                if (user.EmailConfirm)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim("UserAvatar",user.Avatar),
                        new Claim("UserJoinDate", user.CreateDate.ToShamsi()),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    await HttpContext.SignInAsync(principal, properties);
                    ViewBag.IsSuccess = true;
                    return Redirect("/");

                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");

                }

            }
            else
                ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }

        #endregion

        #region LogOut
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

        #region ActiveAccount
        [Route("/activeaccount/{id}")]
        public async Task<IActionResult> ActiveAccount(string id)
        {
            UserVM user = await _accountService.ActiveAccountServiceAsync(id);
            TempData["emailConfirm"] = true;
            return RedirectToAction("login");
        }
        #endregion

        #region ForgotPassword

        #region ForGotPassword
        [Route("/forgotpassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("/forgotpassword")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (!ModelState.IsValid) { return View(forgotPassword); };

            var user = await _accountService.GetUserByEmailServiceAsync(forgotPassword.Email);

            if (user != null)
            {
                string body = _viewRender.RenderToStringAsync("_ForgotPasswordEmail", user);
                SendEmail.Send(user.Email, "فعالسازی", body);
                ViewBag.IsSended = true;
                return View();
            }
            ModelState.AddModelError("Email", "حساب کاربری یافت نشد");
            return View(forgotPassword);
        }

        #endregion

        #endregion

        #region ResetPassword
        [Route("/reset-password/{activeCode}")]
        public async Task<IActionResult> ResetPassword(string activeCode)
        {
            return View();
        }

        [HttpPost("/reset-password/{activeCode}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string activeCode, ResetPasswordVM reset)
        {
            if (!ModelState.IsValid) { return View(reset); };
            var isSuccess = await _accountService.ResetPasswordServiceAsync(activeCode, reset);
            TempData["isResetPasswordSuccess"] = isSuccess;
            return Redirect("/login");
        }
        #endregion

        #region Profile
        [HttpGet("profile/{id:int}/{name}")]
        public async Task<IActionResult> Profile(int id,string name)
        {
            ProfileVM profileInfo=await _userService.ProfileInfoServiceAsync(id);
            await _userService.UpdateVisitProfileServiceAsync(id);
            ViewBag.Followins = await _userService.FollowingsServiceAsync();
            ViewBag.ProfileSelectedAnswers = await _questionService.ProfileSelectedAswersServiceAsync(id);
            ViewBag.ProfileSelectedQuestions = await _questionService.ProfileSelectedQuestionsServiceAsync(id);
            return View(profileInfo);
        }
        [HttpGet("follow-user/{id}")]
        [Authorize]

        public async Task<IActionResult> Follow(int id)
        {
            int follwingId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id == follwingId) return BadRequest();

            await _userService.FollowUserServiceAsync(id, follwingId);
            return Json(new { status = "success" });
        }


        #endregion



        #endregion


    }
}
