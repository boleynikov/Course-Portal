// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API.Controllers.Abstract;
    using API.Controllers.Helper;
    using Domain;
    using Domain.CourseMaterials;
    using Domain.Enum;
    using Services;
    using Services.Interface;

    /// <summary>
    /// User Controller.
    /// </summary>
    public class UserController : IController
    {
        private const string _homePage = "home";
        private const string _exitCommand = "0";
        private const string _userPage = "1";
        private const string _createCourseCommand = "3";
        private const string _deleteCourseCommand = "4";
        private const string _openCourseCommand = "5";
        private const string _stopAddingCommand = "6";

        private const string _articleInputCase = "1";
        private const string _publicationInputCase = "2";
        private const string _videoInputCase = "3";

        private readonly IService<Course> _courseService;
        private readonly IService<Material> _materialService;
        private readonly IService<User> _userService;
        private readonly IAuthorizationService _authorizedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="materialService">Material service instance.</param>
        /// <param name="userService">User service instance.</param>
        /// <param name="authorizedUser">Current authorized user service</param>
        public UserController(
            IService<Course> courseService,
            IService<Material> materialService,
            IService<User> userService,
            IAuthorizationService authorizedUser)
        {
            _courseService = courseService;
            _materialService = materialService;
            _userService = userService;
            _authorizedUser = authorizedUser;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            return View();
        }

        private string View()
        {
            var currentUser = _authorizedUser.GetCurrentAccount();
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
                        var course = courseKeyValue.Course;
                        var progress = courseKeyValue.Progress;
                        Console.WriteLine("\t|{0, 2}.| {1,-40} | {2, 5}, {3, 3} %", course.Id, course.Name, progress.State, progress.Percentage);
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Щоб створити курс - введіть {_createCourseCommand}\n" +
                                  $"Щоб переглянути якийсь курс - введіть {_openCourseCommand}\n" +
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
                        if (ValidateCourse(Console.ReadLine(), out Course course))
                        {
                            page = new CourseController(_userService, _courseService, _authorizedUser, new OpenedCourseService(course), _userPage).Launch();
                        }

                        break;
                    case _deleteCourseCommand:
                        Console.Write("Введіть номер курсу: ");
                        if (ValidateCourse(Console.ReadLine(), out course))
                        {
                            _authorizedUser.RemoveCourse(course.Id);
                            _userService.Save();
                        }

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
                    if ((int)skillKind > 7 || (int)skillKind < 0)
                    {
                        Console.WriteLine("Такої навички немає\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                        continue;
                    }

                    if (points <= 0)
                    {
                        Console.WriteLine("Не можна вказувати стільки поінтів\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                        continue;
                    }

                    var newSkill = new Skill { Name = skillKind, Points = points };
                    courseSkills.Add(newSkill);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Не вірний формат вводу\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                }
            }

            return courseSkills;
        }

        private void CreateCourse()
        {
            Console.Clear();

            Console.Write("Введіть назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());

            Console.Write("Введіть опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());

            int id = _courseService.GetAll().ToList().Count + 1;
            var course = new Course(id, name, description);
            List<Material> materials = new ();
            if (_authorizedUser.GetCurrentAccount().UserMaterials.Count > 0)
            {
                Console.WriteLine("Чи бажаете ви додати вже створені матеріали?\n" +
                                  "1 - так\n" +
                                  "2 - ні");
                string cmd = UserInput.NotEmptyString(() => Console.ReadLine());
                if (cmd == "1")
                {
                    materials = AddExistingMaterials(course);
                }
                else
                {
                    materials = CreateMaterials();
                }
            }

            var skills = CreateSkills();

            foreach (var material in materials)
            {
                course.CourseMaterials.Add(material);
            }

            foreach (var skill in skills)
            {
                course.CourseSkills.Add( new Skill { Name = skill.Name, Points = skill.Points });
            }

            _authorizedUser.AddCourse(course);
            _userService.Save();
            _courseService.Add(course);
            Console.Write("Курс успішно додано. Натисніть Enter");
            Console.ReadLine();
        }

        private List<Material> AddExistingMaterials(Course course)
        {
            var userMaterials = _authorizedUser.GetCurrentAccount().UserMaterials;
            var newListMaterials = new List<Material>();
            Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
            userMaterials.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

            var strMaterialsIds = UserInput.NotEmptyString(() => Console.ReadLine());
            var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
            listMaterialsIds.ForEach((stringMatId) =>
            {
                if (ValidateMaterial(stringMatId, out Material material))
                {
                    newListMaterials.Add(material);
                }
            });

            return newListMaterials;
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
                cmdLine = UserInput.NotEmptyString(() => Console.ReadLine());
                switch (cmdLine)
                {
                    case _articleInputCase:
                        Console.Write("Введіть назву статті: ");
                        string title = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть дату публікації статті: ");
                        DateTime date = UserInput.ValidDateTime(() => Console.ReadLine());
                        Console.Write("Введіть посиланя на статтю: ");
                        string link = UserInput.NotEmptyString(() => Console.ReadLine());

                        int id = _materialService.GetAll().ToList().Count + 1;
                        var articleMaterial = new ArticleMaterial(id, title, date, link);

                        _authorizedUser.GetCurrentAccount().UserMaterials.Add(articleMaterial);
                        _materialService.Add(articleMaterial);
                        courseMaterials.Add(articleMaterial);
                        break;
                    case _publicationInputCase:
                        Console.Write("Введіть назву публікації: ");
                        title = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть автора публікації: ");
                        string author = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть кількість сторінок публікації: ");
                        int pageCount = UserInput.ValidInt(() => Console.ReadLine());
                        Console.Write("Введіть формат файлу публікації: ");
                        string format = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть дату публікації: ");
                        date = UserInput.ValidDateTime(() => Console.ReadLine());

                        id = _materialService.GetAll().ToList().Count + 1;
                        var publicationMaterial = new PublicationMaterial(id, title, author, pageCount, format, date);

                        _authorizedUser.GetCurrentAccount().UserMaterials.Add(publicationMaterial);
                        _materialService.Add(publicationMaterial);
                        courseMaterials.Add(publicationMaterial);
                        break;
                    case _videoInputCase:
                        Console.Write("Введіть назву відео: ");
                        title = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть довжину відео: ");
                        float duration = UserInput.ValidFloat(() => Console.ReadLine());
                        Console.Write("Введіть якість відео: ");
                        int quality = UserInput.ValidInt(() => Console.ReadLine());

                        id = _materialService.GetAll().ToList().Count + 1;
                        var videoMaterial = new VideoMaterial(id, title, duration, quality);

                        _authorizedUser.GetCurrentAccount().UserMaterials.Add(videoMaterial);
                        _materialService.Add(videoMaterial);
                        courseMaterials.Add(videoMaterial);
                        break;
                    case _stopAddingCommand:
                        Console.WriteLine("Матеріали додано");
                        break;
                    default:
                        Console.WriteLine("На жаль це не тип матеріалу\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                        break;
                }

                Console.Clear();
            }

            _userService.Save();
            return courseMaterials;
        }

        private bool ValidateMaterial(string strMaterialId, out Material material)
        {
            if (int.TryParse(strMaterialId, out int materialId))
            {
                try
                {
                    material = _authorizedUser.GetCurrentAccount().UserMaterials.FirstOrDefault(c => c.Id == materialId)
                        ?? throw new ArgumentOutOfRangeException(nameof(materialId));
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    material = null;
                    Console.WriteLine($"Немає матеріалу з таким ідентифікатором {materialId}\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                material = null;
                Console.WriteLine("Неправильний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return false;
            }
        }

        private bool ValidateCourse(string strCourseId, out Course course)
        {
            if (int.TryParse(strCourseId, out int courseId))
            {
                try
                {
                    course = _authorizedUser.GetCurrentAccount().UserCourses.FirstOrDefault(c => c.Course.Id == courseId).Course
                        ?? throw new ArgumentOutOfRangeException(nameof(courseId));
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    course = null;
                    Console.WriteLine("Немає курсу з таким ідентифікатором\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                course = null;
                Console.WriteLine("Неправильний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return false;
            }
        }
    }
}
