using Domain;
using System;

namespace API.View
{
    /// <summary>
    /// Course console view
    /// </summary>
    public class CourseView
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

            Console.WriteLine($"Курс: {currentCourse.Name}");
            var pulledCourseTuple = currentUser.UserCourses.Find(c => c.Course.Id == currentCourse.Id);
            if (pulledCourseTuple.Course != null)
            {
                Console.WriteLine($"\tВаш прогрес: {pulledCourseTuple.Progress.State} {pulledCourseTuple.Progress.Percentage} %");
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
        }
    }
}
