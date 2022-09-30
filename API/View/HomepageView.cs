using Domain;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAPI.View
{
    /// <summary>
    /// Home Page console view
    /// </summary>
    public static class HomepageView
    {
        /// <summary>
        /// Show home page to console
        /// </summary>
        /// <param name="courses">All courses in repo</param>
        /// <param name="isAuthorized"></param>
        /// <param name="userName">Current user name</param>
        public static void Show(IEnumerable<Course> courses, bool isAuthorized, string userName = "")
        {
            if (courses == null)
            {
                throw new ArgumentNullException(nameof(courses));
            }

            var courseList = courses.ToList();
            Console.Clear();
            Console.WriteLine("Вітаємо на Educational Portal\n" +
                              "Список наявних курсів:");
            if (courseList.Count <= 0)
            {
                Console.WriteLine("\tНаразі репозиторій курсів пустий");
            }
            else
            {
                for (int i = 0; i < courseList.Count; i++)
                {
                    var description = courseList[i].Description.Length > 50
                        ? $"{courseList[i].Description.Substring(0, 50)}..."
                        : courseList[i].Description;
                    Console.WriteLine("\t|{0, 2}.| {1,-40} | {2,5}", courseList[i].Id, courseList[i].Name, description);
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
