// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Linq;
    using API.Controllers.Abstract;
    using API.Controllers.Helper;
    using API.View;
    using Domain;
    using Services;
    using Services.Interface;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : IController
    {
        private readonly IService<Course> _courseService;
        private readonly IService<User> _userService;
        private readonly IAuthorizationService _authorizedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="userService">User service instance.</param>
        /// <param name="authService">Authorization service instance.</param>
        public HomeController(
            IService<Course> courseService,
            IService<User> userService,
            IAuthorizationService authService)
        {
            _courseService = courseService;
            _userService = userService;
            _authorizedUser = authService;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            var courses = _courseService.GetAll().ToList();
            string page = Command.HomePage;

            while (page == Command.HomePage)
            {
                var currentUser = _authorizedUser.Get();
                string cmdLine;
                if (currentUser == null)
                {
                    HomepageView.Show(courses, false);
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case Command.LoginCommand:
                            Login();
                            break;
                        case Command.RegisterCommand:
                            Register();
                            break;
                        case Command.ExitCommand:
                            page = Command.ExitCommand;
                            break;
                    }
                }
                else
                {
                    HomepageView.Show(courses, true, currentUser.Name);
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case Command.UserPage:
                            page = Command.UserPage;
                            break;
                        case Command.AddCourseCommand:
                            AddCourse();
                            break;
                        case Command.OpenCourseCommand:
                            page = OpenCourse();
                            break;
                        case Command.LogoutCommand:
                            _authorizedUser.Logout();
                            break;
                        case Command.ExitCommand:
                            page = Command.ExitCommand;
                            break;
                        default:
                            return page;
                    }
                }
            }

            return page;
        }

        private void Register()
        {
            Console.Write("Введіть своє ім'я: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть електронну адресу: ");
            string email = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть пароль: ");
            string password = UserInput.NotEmptyString(() => Console.ReadLine());
            _authorizedUser.Register(name, email, password);
        }

        private void Login()
        {
            Console.Write("Введіть електронну адресу: ");
            string email = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть пароль: ");
            string password = UserInput.NotEmptyString(() => Console.ReadLine());
            var loginResult = _authorizedUser.Login(email, password);
            if (loginResult)
            {
                Console.WriteLine($"З поверненням {_authorizedUser.Get().Name}\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Невірний email чи пароль");
                Console.ReadLine();
            }
        }

        private void AddCourse()
        {
            Console.Write("Введіть номер курсу: ");
            if (ValidateCourse(Console.ReadLine(), out Course course))
            {
                _authorizedUser.AddCourse(course);
                _userService.Save();
            }
        }

        private string OpenCourse()
        {
            Console.Write("Введіть номер курсу: ");
            if (ValidateCourse(Console.ReadLine(), out Course course))
            {
                return new CourseController(_userService, _courseService, _authorizedUser, new OpenedCourseService(course)).Launch();
            }

            return Command.BackCommand;
        }

        private bool ValidateCourse(string strCourseId, out Course course)
        {
            if (int.TryParse(strCourseId, out int courseId))
            {
                try
                {
                    course = _courseService.GetByIndex(courseId);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    course = null;
                    Console.WriteLine($"Немає курсу з таким ідентифікатором {courseId}\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                course = null;
                Console.WriteLine("Неправильний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return false;
            }
        }
    }
}
