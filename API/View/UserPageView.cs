using Domain;
using Services.Helper;
using System;
using System.Collections.Generic;

namespace API.View
{
    /// <summary>
    /// User Page console view
    /// </summary>
    public class UserPageView
    {
        /// <summary>
        /// Show user page to console
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userCourses"></param>
        public static void Show(User currentUser, List<(Course, CourseProgress)> userCourses)
        {
            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses));
            }

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            Console.WriteLine($"Обліковий запис\n" +
                                  $"Ім'я: {currentUser.Name}\n" +
                                  $"Email: {currentUser.Email}\n");
            if (userCourses.Count <= 0)
            {
                Console.WriteLine("У вас ще немає доданих чи створених курсів.");
            }
            else
            {
                Console.WriteLine($"Кількість курсів користувача: {userCourses.Count}\n" +
                                   "Список наявних курсів:");
                foreach (var courseKeyValue in currentUser.UserCourses)
                {
                    var course = courseKeyValue.Course;
                    var progress = courseKeyValue.Progress;
                    Console.WriteLine("\t|{0, 2}.| {1,-40} | {2, 5}, {3, 3} %", course.Id, course.Name, progress.State, progress.Percentage);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Щоб створити курс - введіть {Command.CreateCourseCommand}\n" +
                              $"Щоб переглянути якийсь курс - введіть {Command.OpenCourseCommand}\n" +
                              $"Щоб видалити курс - введіть {Command.DeleteCourseCommand}\n" +
                              $"Щоб повернутися назад - введіть {Command.BackCommand}");
        }
    }
}
