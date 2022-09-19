using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspAPI.Models;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interface;
using Course = AspAPI.Models.Course;
using User = AspAPI.Models.User;

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
        [HttpGet]
        public IActionResult UserProfile()
        {
            return View(_authorizedUser.Account);
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCourseForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GoToCourseForm(Models.Course course)
        {
            return View("CreateCourseForm", course);
        }

        public IActionResult Logout()
        {
            _authorizationService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var newUser = await _authorizationService.Register(user?.Name, user.Email, user.Password);
                return View("RegistrationConfirm", newUser);
            }

            return View("RegisterForm");
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                await _authorizationService.Login(user?.Email, user.Password);
                return RedirectToAction("Index", "Home");
            }

            return View("LoginForm");
        }

    }
}
