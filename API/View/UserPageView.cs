using Domain;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAPI.View
{
    /// <summary>
    /// User Page console view
    /// </summary>
    public static class UserPageView
    {
        /// <summary>
        /// Show user page to console
        /// </summary>
        /// <param name="currentUser">Current authorized user</param>
        /// <param name="userCourses">Courses in user list</param>
        public static void Show(User currentUser, IEnumerable<Course> userCourses)
        {
            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            if (userCourses == null)
            {
                throw new ArgumentNullException(nameof(userCourses));
            }

            var courseList = userCourses.ToList();
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

            var userCoursesDictionary = courseList.Zip(currentUser.UserCourses.Values,
                                                        (k, v) => new { v, k }).ToDictionary(x => x.k, x => x.v);

            if (courseList.Count <= 0)
            {
                Console.WriteLine("У вас ще немає доданих чи створених курсів.");
            }
            else
            {
                Console.WriteLine($"Кількість курсів користувача: {courseList.Count}\n" +
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
