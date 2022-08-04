using API.Controllers.Abstract;
using Domain;
using Services.Abstract;
using System;

namespace API.Controllers
{
    public class CourseController : IController
    {
        private readonly IService<User> userService;
        private readonly Course course;
        private int userId;
        public CourseController(IService<User> userService, int userId, Course course)
        {
            this.userService = userService;
            this.course = course;
            this.userId = userId;
        }
        public string Launch()
        {
            string page = "course";
            while (page == "course")
            {
                Console.Clear();
                var currentUser = userService.GetByIndex(userId);
                Console.WriteLine($"Курс {course.Name}");
                var pulledCoursePair = currentUser.UserCourses.Find(c => c.Item1.Id == course.Id);
                if (pulledCoursePair.Item1 != null)
                {
                    Console.WriteLine($"\tВаш прогрес: {pulledCoursePair.Item2.State} {pulledCoursePair.Item2.Percentage} %");
                }

                Console.WriteLine($"Опис: {course.Description}");
                Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
                foreach(var skill in course.CourseSkills)
                {
                    Console.WriteLine($"{skill.Name} - {skill.Points} lvl.");
                }
                Console.WriteLine("Щоб додати до свого списку курс - введіть \"add\"");
                Console.WriteLine("Щоб повернутися назад - введіть \"home\"");
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "add":
                        currentUser.AddCourse(course);
                        userService.Save();
                        break;
                    case "home":
                        page = "home";
                        break;
                }
            }
            return page;
        }
    }
}
