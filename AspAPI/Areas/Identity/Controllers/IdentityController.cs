using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AspAPI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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
        [HttpGet]
        public IActionResult UserProfile()
        {
            return View(_authorizedUser.Account);
        }

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

        [HttpPost]
        public IActionResult Logout()
        {
            _authorizationService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            if (user == null)
            {
                return View("RegisterForm");
            }

            if (ModelState.IsValid)
            {
                var registrationResult = await _authorizationService.Register(user.Name, user.Email, user.Password);
                if (registrationResult)
                {
                    return View("RegistrationConfirm", user);
                }

                ModelState.AddModelError("", "There is already a user with this email");
            }

            return View("RegisterForm");
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel user)
        {
            if (user == null)
            {
                return View("LoginForm");
            }
            if (ModelState.IsValid)
            {
                var loginResult = await _authorizationService.Login(user.Email, user.Password);
                if (loginResult)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect username or password");
            }

            return View("LoginForm");
        }

    }
}
