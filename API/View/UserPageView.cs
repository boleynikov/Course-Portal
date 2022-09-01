using Domain;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.View
{
    /// <summary>
    /// User Page console view
    /// </summary>
    public static class UserPageView
    {
        /// <summary>
        /// Show user page to console
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userCourses"></param>
        public static void Show(User currentUser, List<Course> userCourses)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses));
            }

            Console.WriteLine($"Обліковий запис\n" +
                                  $"Ім'я: {currentUser.Name}\n" +
                                  $"Email: {currentUser.Email}\n");

            if (currentUser.UserSkills.Count <= 0)
            {
                Console.WriteLine("Ви ще не освоїли ніяких навичок\n");
            }
            else
            {
                Console.WriteLine("Навички, якими ви володієте:");
                foreach (var skill in currentUser.UserSkills)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", skill.Name, skill.Points);
                }
            }

            var userCoursesDictionary = userCourses.Zip(currentUser.UserCourses.Values.Reverse(),
                                                        (k, v) => new { v, k }).ToDictionary(x => x.k, x => x.v);

            if (userCourses.Count <= 0)
            {
                Console.WriteLine("У вас ще немає доданих чи створених курсів.");
            }
            else
            {
                Console.WriteLine($"Кількість курсів користувача: {userCourses.Count}\n" +
                                   "Список наявних курсів:");
                foreach (var courseKeyValue in userCoursesDictionary)
                {
                    var course = courseKeyValue.Key;
                    var progress = courseKeyValue.Value;
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
