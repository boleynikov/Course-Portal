using Domain;
using Services.Helper;
using System;
using System.Linq;

namespace ConsoleAPI.View
{
    /// <summary>
    /// Course console view
    /// </summary>
    public static class CoursePageView
    {
        /// <summary>
        /// Show to console
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentCourse"></param>
        public static void Show(User currentUser, Course currentCourse)
        {
            if (currentCourse == null)
            {
                throw new ArgumentNullException(nameof(currentCourse));
            }

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            Console.WriteLine($"Курс: {currentCourse.Name} | *{currentCourse.Status}\n");
            var pulledCourseTuple = currentUser.UserCourses.FirstOrDefault(c => c.Key == currentCourse.Id);
            if (pulledCourseTuple.Value != null)
            {
                Console.WriteLine($"\tВаш прогрес: {pulledCourseTuple.Value.State} {pulledCourseTuple.Value.Percentage} %");
            }

            Console.WriteLine($"Опис: {currentCourse.Description}\n" +
                               "Матеріали курсу:");
            foreach (var material in currentCourse.CourseMaterials)
            {
                Console.WriteLine("\t{0, 2} {1,20} | {2,5}", material.Id, material.Type, material.Title);
            }

            Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
            foreach (var skill in currentCourse.CourseSkills)
            {
                Console.WriteLine("\t{0,20} | {1,5}", skill.Name, skill.Points);
            }

            if (currentUser.UserCourses.FirstOrDefault(c => c.Key == currentCourse.Id).Key != 0)
            {
                Console.WriteLine($"Щоб продовжити курс - введіть {Command.OpenCourseCommand}\n" +
                                  $"Щоб змінити назву опис чи додати матеріали до курсу - введіть {Command.EditCommand}\n" +
                                  $"Щоб повернутися назад - введіть {Command.BackCommand}\n");
            }
            else
            {
                Console.WriteLine($"Щоб додати до свого списку курс - введіть {Command.AddCourseCommand}\n" +
                                  $"Щоб змінити назву опис чи додати матеріали до курсу - введіть {Command.EditCommand}\n" +
                                  $"Щоб повернутися назад - введіть {Command.BackCommand}\n");
            }
        }

        /// <summary>
        /// Botttom navigation
        /// </summary>
        public static void EditNavigationView()
        {
            Console.WriteLine("Введіть цифри у відповідності до того що саме ви хочете відредагувати\n" +
                              "через кому пробіл [, ]\n" +
                              $"{Command.EditCourseName} - змінити назву\n" +
                              $"{Command.EditCourseDescription} - змінити опис\n" +
                              $"{Command.AddCourseMaterials} - додати матеріали із вже завантажених користувачем\n" +
                              $"{Command.DeleteCourseMaterial} - видалити матеріал із курсу\n" +
                              $"{Command.AddNewOrEditSkill} - додати нову навичку до курсу або відредагувати навичку\n" +
                              $"{Command.DeleteSkill} - видалити якусь навичку з курсу\n");
        }
    }
}
