// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using API.Controllers.Abstract;
    using Domain;
    using Services.Abstract;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : IController
    {
        private readonly IService<Course> _courseService;
        private readonly IService<User> _userService;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="userService">User service instance.</param>
        /// <param name="authService">Authentication service instance.</param>
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
            string page = "home";

            while (page == "home")
            {
                Console.Clear();
                Console.WriteLine("Вітаємо на Educational Portal");
                Console.WriteLine("Список наявних курсів:");
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
                    Console.WriteLine("Увійдіть до свого облікового запису ввівши \"login\"");
                    Console.WriteLine("Якщо такого ще немає, введіть \"register\"");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case "login":
                            Console.Write("Введіть електронну адресу: ");
                            string email = Console.ReadLine();
                            Console.Write("Введіть пароль: ");
                            string password = Console.ReadLine();
                            var loginResult = _authService.Login(email, password);
                            if (loginResult)
                            {
                                Console.WriteLine($"З поверненням {_authService.GetCurrentAccount().Name}");
                                Console.WriteLine("Натисніть Enter");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("Невірний email чи пароль");
                                Console.ReadLine();
                            }

                            break;
                        case "register":
                            Console.Write("Введіть своє ім'я: ");
                            string name = Console.ReadLine();
                            Console.Write("Введіть електронну адресу: ");
                            email = Console.ReadLine();
                            Console.Write("Введіть пароль: ");
                            password = Console.ReadLine();
                            _authService.Register(name, email, password);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"\tОбліковий запис {currentUser.Name}");
                    Console.WriteLine("Щоб переглянути якийсь курс - введіть \"open\"");
                    Console.WriteLine("Щоб додати до свого списку курс - введіть \"add\"");
                    Console.WriteLine("Щоб переглянути свою сторінку - введіть \"user\"");
                    Console.WriteLine("Щоб вийти звідси - введіть \"exit\"");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case "user":
                            page = "user";
                            break;
                        case "add":
                            Console.Write("Введіть номер курсу: ");
                            int courseId = int.Parse(Console.ReadLine()) - 1;
                            currentUser.AddCourse(courses[courseId]);
                            _userService.Save();
                            break;
                        case "open":
                            Console.Write("Введіть номер курсу: ");
                            courseId = int.Parse(Console.ReadLine()) - 1;
                            page = new CourseController(_userService, _courseService, currentUser.Id, courses[courseId]).Launch();
                            break;
                        case "exit":
                            page = "exit";
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
