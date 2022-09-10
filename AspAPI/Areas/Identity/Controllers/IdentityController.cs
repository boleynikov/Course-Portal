using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult RegistrationConfirm(string Name, string Email, string Password)
        {
            _authorizationService.Register(Name, Email, Password);
            return View(_authorizedUser.Account);
        }
        [HttpPost]
        public IActionResult LoginUser(string Email, string Password)
        {
            _authorizationService.Login(Email, Password);
            return RedirectToAction("Index", "Home");
        }
    }
}
