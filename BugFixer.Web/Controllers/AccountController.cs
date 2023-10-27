﻿using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.ViewModels.Account;
using BugFixer.Application.ViewModels.User;
using EShop.Application.Convertors;
using EShop.Application.Senders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugFixer.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly IAccountService _accountService;
        private IViewRenderService _viewRender;
        #endregion

        #region Constructor
        public AccountController(IAccountService accountService, IViewRenderService viewRender)
        {
            _accountService = accountService;
            _viewRender = viewRender;
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
        [Route("activeaccount/{id}")]
        public async Task<IActionResult> ActiveAccount(string id)
        {
            UserVM user = await _accountService.ActiveAccountServiceAsync(id);

            return RedirectToAction("login", user.EmailConfirm);
        }
        #endregion

        #endregion


    }
}