using API.Controllers.Abstract;
using Domain;
using Services.Abstract;
using System;

namespace API.Controllers
{
    public class CourseController : IController
    {
        private readonly IService<User> userService;
        private readonly IService<Course> courseService;
        private readonly Course course;
        private int userId;
        private string redirectPage;
        public CourseController(IService<User> userService, 
                                IService<Course> courseService, 
                                int userId, 
                                Course course,
                                string redirectPage = "home")
        {
            this.userService = userService;
            this.courseService = courseService;
            this.course = course;
            this.userId = userId;
            this.redirectPage = redirectPage;
        }
        public string Launch()
        {
            string page = "course";
            while (page == "course")
            {
                Console.Clear();
                var currentUser = userService.GetByIndex(userId);
                Console.WriteLine($"Курс: {course.Name}");
                var pulledCoursePair = currentUser.UserCourses.Find(c => c.Item1.Id == course.Id);
                if (pulledCoursePair.Item1 != null)
                {
                    Console.WriteLine($"\tВаш прогрес: {pulledCoursePair.Item2.State} {pulledCoursePair.Item2.Percentage} %");
                }

                Console.WriteLine($"Опис: {course.Description}");
                Console.WriteLine("Матеріали курсу:");
                foreach (var material in course.CourseMaterials)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", material.Type, material.Title);
                }
                Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
                foreach(var skill in course.CourseSkills)
                {
                    Console.WriteLine("\t{0,20} | {1,5}",  skill.Name, skill.Points);
                }
                Console.WriteLine("Щоб додати до свого списку курс - введіть \"add\"");
                Console.WriteLine("Щоб змінити назву чи опис курсу - введіть \"edit\"");
                Console.WriteLine("Щоб повернутися назад - введіть \"back\"");
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "add":
                        currentUser.AddCourse(course);
                        userService.Save();
                        break;
                    case "edit":
                        Console.Write("Введіть нову назву курсу: ");
                        string name = Console.ReadLine();
                        Console.Write("Введіть новий опис курсу: ");
                        string description = Console.ReadLine();
                        course.Update(name, description, course.CourseMaterials, course.CourseSkills);
                        currentUser.UpdateCourse(course);
                        courseService.Update(course);
                        userService.Update(currentUser);
                        break;
                    case "back":
                        page = redirectPage;
                        break;
                }
            }
            return page;
        }
    }
}
