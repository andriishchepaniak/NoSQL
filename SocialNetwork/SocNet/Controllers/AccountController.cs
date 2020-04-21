using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SocNet.Models;
using SocNet.Services;

namespace SocNet.Controllers
{
    public class AccountController : Controller
    {
        private UserService _userService;

        public AccountController()
        {
            _userService = new UserService();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool hasAccess = await _userService.Login(model.Email, model.Password);

                if (hasAccess)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Posts");
                }
                ModelState.AddModelError("", "Incorrect email or password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUserExists = await _userService.IsExist(model.Email);
                if (!isUserExists)
                {

                    if (model.Password == model.PasswordConfirm)
                    {
                        var user = new UserViewModel()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            UserName = model.Username,
                            Password = _userService.Hash(model.Password),
                            Followers = new List<string>(),
                        };
                        
                        await _userService.Create(user);
                    }

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Posts");
                }
                else
                    ModelState.AddModelError("", "Incorrect user name or password");
            }
            return View(model);
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}