// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using API.Controllers.Abstract;
    using Domain;
    using Domain.CourseMaterials;
    using Domain.Enum;
    using Services.Interface;

    /// <summary>
    /// User Controller.
    /// </summary>
    public class UserController : IController
    {
        private const string _exitCommand = "0";
        private const string _userPage = "1";
        private const string _homePage = "home";
        private const string _stopAddingCommand = "2";
        private const string _createCourseCommand = "3";
        private const string _deleteCourseCommand = "4";
        private const string _openCourseCommand = "5";

        private const string _articleInputCase = "1";
        private const string _publicationInputCase = "2";
        private const string _videoInputCase = "3";

        private readonly IService<Course> _courseService;
        private readonly IService<Material> _materialService;
        private readonly IService<User> _userService;
        private readonly int _userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="materialService">Material service instance.</param>
        /// <param name="userService">User service instance.</param>
        /// <param name="userId">Current user id.</param>
        public UserController(
            IService<Course> courseService,
            IService<Material> materialService,
            IService<User> userService,
            int userId)
        {
            _courseService = courseService;
            _materialService = materialService;
            _userService = userService;
            _userId = userId;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            return View();
        }

        private string View()
        {
            var currentUser = _userService.GetByIndex(_userId);
            string page = _userPage;

            while (page == _userPage)
            {
                Console.Clear();
                Console.WriteLine($"Обліковий запис\n" +
                                  $"Ім'я: {currentUser.Name}\n" +
                                  $"Email: {currentUser.Email}\n");
                if (currentUser.UserCourses.Count <= 0)
                {
                    Console.WriteLine("У вас ще немає доданих чи створених курсів.");
                }
                else
                {
                    Console.WriteLine($"Кількість курсів користувача: {currentUser.UserCourses.Count}\n" +
                                       "Список наявних курсів:");
                    foreach (var courseKeyValue in currentUser.UserCourses)
                    {
                        var course = courseKeyValue.Item1;
                        var progress = courseKeyValue.Item2;
                        Console.WriteLine("\t|{0, 2}.| {1,-40} | {2, 5}, {3, 3} %", course.Id, course.Name, progress.State, progress.Percentage);
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Щоб створити курс - введіть {_createCourseCommand}\n" +
                                     $"Щоб видалити курс - введіть {_deleteCourseCommand}\n" +
                                     $"Щоб повернутися назад - введіть {_exitCommand}");

                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case _createCourseCommand:
                        CreateCourse();
                        break;
                    case _openCourseCommand:
                        Console.Write("Введіть номер курсу: ");
                        int.TryParse(Console.ReadLine(), out int courseId);
                        courseId--;
                        page = new CourseController(_userService, _courseService, currentUser.Id, _courseService.GetByIndex(courseId), _userPage).Launch();
                        break;
                    case _deleteCourseCommand:
                        Console.Write("Введіть номер курсу: ");
                        courseId = int.Parse(Console.ReadLine());
                        currentUser.RemoveCourse(courseId);
                        _userService.Save();
                        break;
                    case _exitCommand:
                        page = _homePage;
                        break;
                }
            }

            return page;
        }

        private List<Skill> CreateSkills()
        {
            List<Skill> courseSkills = new ();
            string cmdLine = string.Empty;

            Console.Clear();
            Console.WriteLine("Оберіть навички, які можна отримати пройшовши курс:");
            while (cmdLine != "stop")
            {
                Console.WriteLine($"Доступні навички:\n" +
                                   "0 - Programming,\n" +
                                   "1 - Music,\n" +
                                   "2 - Physics,\n" +
                                   "3 - HealthCare,\n" +
                                   "4 - TimeManagment,\n" +
                                   "5 - Communication,\n" +
                                   "6 - Illustration,\n" +
                                   "7 - Photo\n" +
                 "Введіть номер навика і кількість поінтів через дорівнює (Ось так: 1 = 3)\n" +
                 $"Або введіть {_stopAddingCommand}, щоб зупинитися");
                cmdLine = Console.ReadLine();
                if (cmdLine == _stopAddingCommand)
                {
                    break;
                }

                string[] skillStr = cmdLine.Split(" = ");
                if (Enum.TryParse(skillStr[0], out SkillKind skillKind) && int.TryParse(skillStr[1], out int points))
                {
                    var newSkill = new Skill { Name = skillKind, Points = points };
                    courseSkills.Add(newSkill);
                    Console.Clear();
                }
            }

            return courseSkills;
        }

        private void CreateCourse()
        {
            Console.Clear();

            Console.Write("Введіть назву курсу: ");
            string name = Console.ReadLine();

            Console.Write("Введіть опис курсу: ");
            string description = Console.ReadLine();

            int id = _courseService.GetAll().Length + 1;
            var course = new Course(id, name, description);

            var materials = CreateMaterials();
            var skills = CreateSkills();

            foreach (var material in materials)
            {
                course.AddMaterial(material);
            }

            foreach (var skill in skills)
            {
                course.AddSkill(skill, skill.Points);
            }

            _userService.GetByIndex(_userId).AddCourse(course);
            _userService.Save();
            _courseService.Add(course);
            Console.Write("Курс успішно додано. Натисніть Enter");
            Console.ReadLine();
        }

        private List<Material> CreateMaterials()
        {
            List<Material> courseMaterials = new ();
            string cmdLine = string.Empty;

            Console.Clear();
            while (cmdLine != _stopAddingCommand)
            {
                Console.WriteLine($"Введіть номер типу матеріалу, який хочете додати до курсу\n" +
                                   "Доступні матеріали:\n" +
                                   "1 - Article,\n" +
                                   "2 - Publication,\n" +
                                   "3 - Video\n" +
                                  $"Або введіть {_stopAddingCommand}, щоб зупинитися");
                Console.Write("Обраний матеріал: ");
                cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case _articleInputCase:
                        Console.Write("Введіть назву статті: ");
                        string title = Console.ReadLine();
                        Console.Write("Введіть дату публікації статті: ");
                        DateTime.TryParse(Console.ReadLine(), out DateTime date);
                        Console.Write("Введіть посиланя на статтю: ");
                        string link = Console.ReadLine();

                        int id = _materialService.GetAll().Length + 1;
                        var articleMaterial = new ArticleMaterial(id, title, date, link);

                        _userService.GetByIndex(_userId).AddMaterial(articleMaterial);
                        _materialService.Add(articleMaterial);
                        courseMaterials.Add(articleMaterial);
                        break;
                    case _publicationInputCase:
                        Console.Write("Введіть назву публікації: ");
                        title = Console.ReadLine();
                        Console.Write("Введіть автора публікації: ");
                        string author = Console.ReadLine();
                        Console.Write("Введіть кількість сторінок публікації: ");
                        int.TryParse(Console.ReadLine(), out int pageCount);
                        Console.Write("Введіть формат файлу публікації: ");
                        string format = Console.ReadLine();
                        Console.Write("Введіть дату публікації: ");
                        DateTime.TryParse(Console.ReadLine(), out date);

                        id = _materialService.GetAll().Length + 1;
                        var publicationMaterial = new PublicationMaterial(id, title, author, pageCount, format, date);

                        _userService.GetByIndex(_userId).AddMaterial(publicationMaterial);
                        _materialService.Add(publicationMaterial);
                        courseMaterials.Add(publicationMaterial);
                        break;
                    case _videoInputCase:
                        Console.Write("Введіть назву відео: ");
                        title = Console.ReadLine();
                        Console.Write("Введіть довжину відео: ");
                        float.TryParse(Console.ReadLine(), out float duration);
                        Console.Write("Введіть якість відео: ");
                        int.TryParse(Console.ReadLine(), out int quality);

                        id = _materialService.GetAll().Length + 1;
                        var videoMaterial = new VideoMaterial(id, title, duration, quality);

                        _userService.GetByIndex(_userId).AddMaterial(videoMaterial);
                        _materialService.Add(videoMaterial);
                        courseMaterials.Add(videoMaterial);
                        break;
                }

                Console.Clear();
            }

            _userService.Save();
            return courseMaterials;
        }
    }
}
