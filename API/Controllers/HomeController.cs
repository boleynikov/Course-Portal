// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using API.Controllers.Abstract;
    using Domain;
    using Services.Interface;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : IController
    {
        private const string _homePage = "home";
        private const string _userPage = "1";
        private const string _loginCommand = "2";
        private const string _registerCommand = "3";
        private const string _addCourseCommand = "4";
        private const string _openCourseCommand = "5";
        private const string _exitCommand = "6";

        private readonly IService<Course> _courseService;
        private readonly IService<User> _userService;
        private readonly IAuthorizationService _authService;

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
            _authService = authService;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            var coursesOnSite = _courseService.GetAll();
            return View(coursesOnSite);
        }

        private string View(Course[] courses)
        {
            string page = _homePage;

            while (page == _homePage)
            {
                Console.Clear();
                Console.WriteLine("Вітаємо на Educational Portal\n" +
                                  "Список наявних курсів:");
                if (courses.Length <= 0)
                {
                    Console.WriteLine("\tНаразі репозиторій курсів пустий");
                }
                else
                {
                    for (int i = 0; i < courses.Length; i++)
                    {
                        Console.WriteLine("\t|{0, 2}.| {1,-40} | {2,5}", i + 1, courses[i].Name, courses[i].Description);
                    }
                }

                Console.WriteLine();
                var currentUser = _authService.GetCurrentAccount();
                string cmdLine;
                if (currentUser == null)
                {
                    Console.WriteLine($"Увійдіть до свого облікового запису ввівши {_loginCommand}\n" +
                                      $"Якщо такого ще немає, введіть {_registerCommand}\n" +
                                      $"Щоб вийти звідси - введіть {_exitCommand}\n");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case _loginCommand:
                            Console.Write("Введіть електронну адресу: ");
                            string email = Console.ReadLine();
                            Console.Write("Введіть пароль: ");
                            string password = Console.ReadLine();
                            var loginResult = _authService.Login(email, password);
                            if (loginResult)
                            {
                                Console.WriteLine($"З поверненням {_authService.GetCurrentAccount().Name}\n" +
                                                   "Натисніть Enter");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("Невірний email чи пароль");
                                Console.ReadLine();
                            }

                            break;
                        case _registerCommand:
                            Console.Write("Введіть своє ім'я: ");
                            string name = Console.ReadLine();
                            Console.Write("Введіть електронну адресу: ");
                            email = Console.ReadLine();
                            Console.Write("Введіть пароль: ");
                            password = Console.ReadLine();
                            _authService.Register(name, email, password);
                            break;
                        case _exitCommand:
                            page = _exitCommand;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"\tОбліковий запис {currentUser.Name}\n" +
                                       $"Щоб переглянути свою сторінку - введіть {_userPage}\n" +
                                       $"Щоб переглянути якийсь курс - введіть {_openCourseCommand}\n" +
                                       $"Щоб додати до свого списку курс - введіть {_addCourseCommand}\n" +
                                       $"Щоб вийти звідси - введіть {_exitCommand}\n");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case _userPage:
                            page = _userPage;
                            break;
                        case _addCourseCommand:
                            Console.Write("Введіть номер курсу: ");
                            int.TryParse(Console.ReadLine(), out int courseId);
                            courseId--;
                            currentUser.AddCourse(courses[courseId]);
                            _userService.Save();
                            break;
                        case _openCourseCommand:
                            Console.Write("Введіть номер курсу: ");
                            int.TryParse(Console.ReadLine(), out courseId);
                            courseId--;
                            page = new CourseController(_userService, _courseService, currentUser.Id, courses[courseId]).Launch();
                            break;
                        case _exitCommand:
                            page = _exitCommand;
                            break;
                        default:
                            return page;
                    }
                }
            }

            return page;
        }
    }
}
