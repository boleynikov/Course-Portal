using Domain;
using System;
using System.Collections.Generic;

namespace API.View
{
    /// <summary>
    /// Home Page console view
    /// </summary>
    public class HomepageView
    {
        /// <summary>
        /// Show home page to console
        /// </summary>
        /// <param name="isAuthorized"></param>
        public static void Show(List<Course> courses, bool isAuthorized, string userName = "")
        {
            if (courses == null)
            {
                throw new ArgumentNullException(nameof(courses));
            }

            Console.Clear();
            Console.WriteLine("Вітаємо на Educational Portal\n" +
                              "Список наявних курсів:");
            if (courses.Count <= 0)
            {
                Console.WriteLine("\tНаразі репозиторій курсів пустий");
            }
            else
            {
                for (int i = 0; i < courses.Count; i++)
                {
                    Console.WriteLine("\t|{0, 2}.| {1,-40} | {2,5}", courses[i].Id, courses[i].Name, courses[i].Description);
                }
            }

            Console.WriteLine();

            if (isAuthorized)
            {
                AuthorizedNavigationView(userName);
            }
            else
            {
                NotAuthorizedNavigationView();
            }
        }

        public static void NotAuthorizedNavigationView()
        {
            Console.WriteLine($"Увійдіть до свого облікового запису ввівши {Command.LoginCommand}\n" +
                                      $"Якщо такого ще немає, введіть {Command.RegisterCommand}\n" +
                                      $"Щоб вийти звідси - введіть {Command.ExitCommand}\n");
        }

        public static void AuthorizedNavigationView(string userName)
        {
            Console.WriteLine($"\tОбліковий запис {userName}\n" +
                                       $"Щоб переглянути свою сторінку - введіть {Command.UserPage}\n" +
                                       $"Щоб переглянути якийсь курс - введіть {Command.OpenCourseCommand}\n" +
                                       $"Щоб додати до свого списку курс - введіть {Command.AddCourseCommand}\n" +
                                       $"Щоб вийти зі свого облікового запису - введіть {Command.LogoutCommand}\n" +
                                       $"Щоб вийти звідси - введіть {Command.ExitCommand}\n");
        }
    }
}
