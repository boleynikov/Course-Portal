// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    using Abstract;
    using Domain;
    using Services;
    using Services.Helper;
    using Services.Interface;
    using Services.Validator;
    using System;
    using System.Linq;
    using View;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : IController
    {
        private readonly IService<Course> _courseService;
        private readonly IService<User> _userService;
        private readonly IAuthorizationService _authorization;
        private readonly IAuthorizedUserService _authorizedUser;
        private readonly Validator _validateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance</param>
        /// <param name="userService">User service instance</param>
        /// <param name="materialService">Material service instance</param>
        /// <param name="authorizationService">Authorization service</param>
        /// <param name="authorizedUserService">Current user service</param>
        /// <param name="validateService">Input validation service</param>
        public HomeController(
            IService<Course> courseService,
            IService<User> userService,
            IAuthorizationService authorizationService,
            IAuthorizedUserService authorizedUserService,
            Validator validateService)
        {
            _courseService = courseService;
            _userService = userService;
            _authorization = authorizationService;
            _authorizedUser = authorizedUserService;
            _validateService = validateService;
        }

        /// <inheritdoc/>
        public async Task<string> Launch()
        {
            var page = Command.HomePage;

            while (page == Command.HomePage)
            {
                var currentUser = _authorizedUser.Account;
                if (currentUser == null)
                {
                    page = await NotAuthorized();
                }
                else
                {
                    page = await Authorized();
                }
            }

            return page;
        }

        private async Task<string> NotAuthorized()
        {
            IEnumerable<Course> courses = await _courseService.GetAll(0);
            HomepageView.Show(courses.ToList(), false);
            var page = Command.HomePage;
            var cmdLine = Console.ReadLine();
            switch (cmdLine)
            {
                case Command.LoginCommand:
                    Console.Write("Введіть електронну адресу: ");
                    var email = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть пароль: ");
                    var password = UserInput.NotEmptyString(() => Console.ReadLine());
                    var loginResult = await _authorization.Login(email, password);
                    if (loginResult)
                    {
                        Console.WriteLine($"З поверненням {_authorizedUser.Account.Name}\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Невірний email чи пароль");
                        Console.ReadLine();
                    }

                    break;
                case Command.RegisterCommand:
                    Console.Write("Введіть своє ім'я: ");
                    var name = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть електронну адресу: ");
                    email = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть пароль: ");
                    password = UserInput.NotEmptyString(() => Console.ReadLine());
                    var registerResult = await _authorization.Register(name, email, password);
                    if (registerResult)
                    {
                        Console.WriteLine($"З поверненням {_authorizedUser.Account.Name}\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Email у неправильному вигляді");
                        Console.ReadLine();
                    }

                    break;
                case Command.ExitCommand:
                    page = Command.ExitCommand;
                    break;
            }

            return page;
        }

        private async Task<string> Authorized()
        {
            IEnumerable<Course> courses = await _courseService.GetAll(0);
            HomepageView.Show(courses.ToList(), true, _authorizedUser.Account.Name);
            var page = Command.HomePage;
            var cmdLine = Console.ReadLine();
            switch (cmdLine)
            {
                case Command.UserPage:
                    page = Command.UserPage;
                    break;
                case Command.AddCourseCommand:
                    Console.Write("Введіть номер курсу: ");
                    if (_validateService.Course.Validate(courses.ToList(), Console.ReadLine(), out Course course))
                    {
                        if (!CourseController.IsCourseNotDeleted(course))
                        {
                            break;
                        }

                        await _authorizedUser.AddCourseToUser(course.Id);
                        await _userService.Save();
                    }

                    break;
                case Command.OpenCourseCommand:
                    Console.Write("Введіть номер курсу: ");
                    if (_validateService.Course.Validate(courses.ToList(), Console.ReadLine(), out course))
                    {
                        page = await new CourseController(_userService, _courseService, _authorizedUser, new OpenedCourseService(course, new Validator())).Launch();
                    }

                    break;
                case Command.LogoutCommand:
                    _authorization.Logout();
                    break;
                case Command.ExitCommand:
                    page = Command.ExitCommand;
                    break;
                default:
                    return page;
            }

            return page;
        }
    }
}
