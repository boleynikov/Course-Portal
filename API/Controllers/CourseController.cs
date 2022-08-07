// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using API.Controllers.Abstract;
    using Domain;
    using Services.Abstract;

    /// <summary>
    /// Course Controller.
    /// </summary>
    public class CourseController : IController
    {
        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;
        private readonly Course _course;
        private readonly int _userId;
        private readonly string _redirectPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="userService">User service instance.</param>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="userId">Id of current user.</param>
        /// <param name="course">Course, that displayed.</param>
        /// <param name="redirectPage">String of page for redirect back.</param>
        public CourseController(
            IService<User> userService,
            IService<Course> courseService,
            int userId,
            Course course,
            string redirectPage = "home")
        {
            _userService = userService;
            _courseService = courseService;
            _course = course;
            _userId = userId;
            _redirectPage = redirectPage;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            string page = "course";
            while (page == "course")
            {
                Console.Clear();
                var currentUser = _userService.GetByIndex(_userId);
                Console.WriteLine($"Курс: {_course.Name}");
                var pulledCoursePair = currentUser.UserCourses.Find(c => c.Item1.Id == _course.Id);
                if (pulledCoursePair.Item1 != null)
                {
                    Console.WriteLine($"\tВаш прогрес: {pulledCoursePair.Item2.State} {pulledCoursePair.Item2.Percentage} %");
                }

                Console.WriteLine($"Опис: {_course.Description}");
                Console.WriteLine("Матеріали курсу:");
                foreach (var material in _course.CourseMaterials)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", material.Type, material.Title);
                }

                Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
                foreach (var skill in _course.CourseSkills)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", skill.Name, skill.Points);
                }

                Console.WriteLine("Щоб додати до свого списку курс - введіть \"add\"");
                Console.WriteLine("Щоб змінити назву чи опис курсу - введіть \"edit\"");
                Console.WriteLine("Щоб повернутися назад - введіть \"back\"");
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "add":
                        currentUser.AddCourse(_course);
                        _userService.Save();
                        break;
                    case "edit":
                        Console.Write("Введіть нову назву курсу: ");
                        string name = Console.ReadLine();
                        Console.Write("Введіть новий опис курсу: ");
                        string description = Console.ReadLine();
                        _course.UpdateInfo(name, description, _course.CourseMaterials, _course.CourseSkills);
                        currentUser.UpdateCourseInfo(_course);
                        _courseService.Update(_course);
                        _userService.Update(currentUser);
                        break;
                    case "back":
                        page = _redirectPage;
                        break;
                }
            }

            return page;
        }
    }
}
