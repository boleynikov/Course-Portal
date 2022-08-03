using Domain;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class HomeController
    {
        public readonly IService<Course> courseService;
        public readonly IAuthenticationService authService;

        public HomeController(IService<Course> service, IAuthenticationService service1)
        {
            courseService = service;
            authService = service1;
        }

        public void Launch()
        {
            var coursesOnSite = courseService.GetAll();
            View(coursesOnSite);
        }

        private void View(Course[] courses) 
        {
            Console.WriteLine("Вітаємо на Educational Portal");
            Console.WriteLine("Список наявних курсів");
            for(int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine($"{i+1}. {courses[i].Name} | {courses[i].Description}");
            }

            Console.WriteLine("Аби перейти до курсу, введіть його номер");
            Console.WriteLine("Увійдіть до свого облікового запису ввівши \"login\"");
            Console.WriteLine("Якщо такого ще немає, введіть \"register\"");
            string cmdLine = String.Empty;
            while (cmdLine != "stop")
            {
                cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "login":
                        Console.Write("Введіть електронну адресу: ");
                        string email = Console.ReadLine();
                        Console.Write("Введіть пароль: ");
                        string password = Console.ReadLine();
                        authService.Login(email, password);
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
                }
            }
        }
    }
}
