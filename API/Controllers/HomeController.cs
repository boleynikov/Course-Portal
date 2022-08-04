using API.Controllers.Abstract;
using Domain;
using Services.Abstract;
using System;

namespace API.Controllers
{
    public class HomeController : IController
    {
        public readonly IService<Course> courseService;
        public readonly IAuthenticationService authService;

        public HomeController(IService<Course> courseService, IAuthenticationService authService)
        {
            this.courseService = courseService;
            this.authService = authService;
        }

        public string Launch()
        {
            var coursesOnSite = courseService.GetAll();
            return View(coursesOnSite);
        }

        private string View(Course[] courses)
        {
            string cmdLine = String.Empty;
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
                        Console.WriteLine($"{i + 1}. {courses[i].Name} | {courses[i].Description}");
                    }
                }

                Console.WriteLine("Аби перейти до курсу, введіть його номер");
                var currentUser = authService.GetCurrentAccount();
                if (currentUser == null)
                {
                    Console.WriteLine("Увійдіть до свого облікового запису ввівши \"login\"");
                    Console.WriteLine("Якщо такого ще немає, введіть \"register\"");
                }
                else
                {
                    Console.WriteLine($"Обліковий запис {currentUser.Name}");
                    Console.WriteLine("Щоб переглянути свої курси, введіть \"showCourses\"");
                }

                cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "login":
                        Console.Write("Введіть електронну адресу: ");
                        string email = Console.ReadLine();
                        Console.Write("Введіть пароль: ");
                        string password = Console.ReadLine();
                        var loginResult = authService.Login(email, password);
                        if(loginResult)
                        {
                            Console.WriteLine($"З поверненням {authService.GetCurrentAccount().Name}");
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
                        authService.Register(name, email, password);                  
                        break;
                    case "showCourses":
                        page = "user";
                        break;
                    default:
                        return page;
                }
            }
            return page;
        }
    }
}
