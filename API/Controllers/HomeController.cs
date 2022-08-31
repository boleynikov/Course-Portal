﻿// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Domain.CourseMaterials;

namespace API.Controllers
{
    using System;
    using System.Linq;
    using Abstract;
    using Domain;
    using Services;
    using Services.Helper;
    using Services.Interface;
    using Services.Validators;
    using View;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : IController
    {
        private readonly IService<Course> _courseService;
        private readonly IService<User> _userService;
        private readonly IService<Material> _materialService;
        private readonly IAuthorizationService _authorization;
        private readonly IAuthorizedUserService _authorizedUser;
        private readonly Validator _validatorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="userService">User service instance.</param>
        /// <param name="authService">Authorization service instance.</param>
        public HomeController(
            IService<Course> courseService,
            IService<User> userService,
            IService<Material> materialService,
            IAuthorizationService authorizationService,
            IAuthorizedUserService authorizedUserService,
            Validator validatorService)
        {
            _courseService = courseService;
            _userService = userService;
            _materialService = materialService;
            _authorization = authorizationService;
            _authorizedUser = authorizedUserService;
            _validatorService = validatorService;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            string page = Command.HomePage;

            while (page == Command.HomePage)
            {
                var currentUser = _authorizedUser.Account;
                if (currentUser == null)
                {
                    page = NotAuthorized();
                }
                else
                {
                    page = Authorized();
                }
            }

            return page;
        }

        private string NotAuthorized()
        {
            var courses = _courseService.GetAll().ToList();
            HomepageView.Show(courses, false);
            string page = Command.HomePage;
            string cmdLine = Console.ReadLine();
            switch (cmdLine)
            {
                case Command.LoginCommand:
                    Console.Write("Введіть електронну адресу: ");
                    string email = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть пароль: ");
                    string password = UserInput.NotEmptyString(() => Console.ReadLine());
                    _authorization.Login(email, password);
                    break;
                case Command.RegisterCommand:
                    Console.Write("Введіть своє ім'я: ");
                    string name = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть електронну адресу: ");
                    email = UserInput.NotEmptyString(() => Console.ReadLine());
                    Console.Write("Введіть пароль: ");
                    password = UserInput.NotEmptyString(() => Console.ReadLine());
                    _authorization.Register(name, email, password);
                    break;
                case Command.ExitCommand:
                    page = Command.ExitCommand;
                    break;
            }

            return page;
        }

        private string Authorized()
        {
            var courses = _courseService.GetAll().ToList();
            HomepageView.Show(courses, true, _authorizedUser.Account.Name);
            string page = Command.HomePage;
            string cmdLine = Console.ReadLine();
            switch (cmdLine)
            {
                case Command.UserPage:
                    page = Command.UserPage;
                    break;
                case Command.AddCourseCommand:
                    Console.Write("Введіть номер курсу: ");
                    if (_validatorService.Course.Validate(courses, Console.ReadLine(), out Course course))
                    {
                        _authorizedUser.AddCourse(course);
                        _userService.Save();
                    }

                    break;
                case Command.OpenCourseCommand:
                    Console.Write("Введіть номер курсу: ");
                    if (_validatorService.Course.Validate(courses, Console.ReadLine(), out course))
                    {
                        page = new CourseController(_userService, _courseService, _materialService, _authorizedUser, new OpenedCourseService(course, new Validator())).Launch();
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