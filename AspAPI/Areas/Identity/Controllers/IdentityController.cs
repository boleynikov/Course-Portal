using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspAPI.Models;
using Domain;
using Services.Interface;

namespace AspAPI.Areas.Identity.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizedUserService _authorizedUser;
        public IdentityController(IAuthorizationService authorizationService, IAuthorizedUserService authorizedUser)
        {
            _authorizationService = authorizationService;
            _authorizedUser = authorizedUser;
        }

        public IActionResult UserProfile()
        {
            return View(_authorizedUser.Account);
        }

        public IActionResult LoginForm()
        {
            return View();
        }

        public IActionResult RegisterForm()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _authorizationService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RegistrationConfirm(Models.User user)
        {
            if (ModelState.IsValid)
            {
                _authorizationService.Register(user?.Name, user.Email, user.Password);
                return View(_authorizedUser.Account);
            }

            return View("RegisterForm");
        }
        [HttpPost]
        public IActionResult LoginUser(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                _authorizationService.Login(user?.Email, user.Password);
                return RedirectToAction("Index", "Home");
            }

            return View("LoginForm");
        }
    }
}
