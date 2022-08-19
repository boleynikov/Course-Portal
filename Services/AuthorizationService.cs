// <copyright file="AuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Domain;
    using Domain.CourseMaterials;
    using Domain.Enum;
    using Helper;
    using Interface;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IService<User> _userService;
        private readonly Validator _validateService;
        private User _account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="service">User service for decorating.</param>
        public AuthorizationService(IService<User> service, Validator validateService)
        {
            _userService = service;
            _validateService = validateService;
        }

        /// <inheritdoc/>
        public void AddCourse(Course newCourse)
        {
            if (newCourse == null)
            {
                throw new ArgumentNullException(nameof(newCourse));
            }

            if (_account.UserCourses.FirstOrDefault(c => c.Key == newCourse.Id).Key != 0)
            {
                Console.WriteLine("Даний курс у вас уже є\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return;
            }

            var item = new KeyValuePair<int, CourseProgress>(newCourse.Id, new CourseProgress() { State = State.NotCompleted, Percentage = 0f });
            _account.UserCourses.Add(item.Key, item.Value);
            _userService.Update(_account);
        }
        /// <inheritdoc/>
        public Course CreateCourse(IService<Course> courseService, IService<Material> materialService)
        {
            if (courseService == null)
            {
                throw new ArgumentNullException(nameof(courseService));
            }

            Console.Write("Введіть назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            int id = courseService.GetAll().ToList().Count + 1;
            var course = new Course(id, name, description);
            List<Material> materials = new();
            if (_account.UserMaterials.ToList().Count > 0)
            {
                Console.WriteLine("Чи бажаете ви додати вже створені матеріали?\n" +
                                  "1 - так\n" +
                                  "2 - ні");
                string cmd = UserInput.NotEmptyString(() => Console.ReadLine());
                if (cmd == "1")
                {
                    materials = AddExistingMaterials().ToList();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Введіть тип матеріалу, який хочете додати до курсу\n" +
                                   "Доступні матеріали:\n" +
                                   $"{Command.ArticleInputCase} - Article,\n" +
                                   $"{Command.PublicationInputCase} - Publication,\n" +
                                   $"{Command.VideoInputCase} - Video\n" +
                                  $"Або введіть {Command.StopAddingCommand}, щоб зупинитися");
                    materials = CreateMaterials(materialService).ToList();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Введіть тип матеріалу, який хочете додати до курсу\n" +
                               "Доступні матеріали:\n" +
                               $"{Command.ArticleInputCase} - Article,\n" +
                               $"{Command.PublicationInputCase} - Publication,\n" +
                               $"{Command.VideoInputCase} - Video\n" +
                              $"Або введіть {Command.StopAddingCommand}, щоб зупинитися");
                materials = CreateMaterials(materialService).ToList();
            }

            string cmdLine = string.Empty;
            while (cmdLine != Command.StopAddingCommand)
            {
                Console.Clear();
                Console.WriteLine("Оберіть навички, які можна отримати пройшовши курс:");
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
                 $"Або введіть {Command.StopAddingCommand}, щоб зупинитися");
                cmdLine = Console.ReadLine();
                if (cmdLine == Command.StopAddingCommand)
                {
                    break;
                }

                Skill skill = CreateSkill(cmdLine);
                if (skill != null)
                {
                    course.CourseSkills.Add(skill);
                }
            }

            foreach (var material in materials)
            {
                course.CourseMaterials.Add(material);
            }

            return course;
        }
        /// <inheritdoc/>
        public IEnumerable<Material> CreateMaterials(IService<Material> materialService)
        {
            if (materialService == null)
            {
                throw new ArgumentNullException(nameof(materialService));
            }

            List<Material> courseMaterials = new();
            string cmdLine = string.Empty;
            while (cmdLine != Command.StopAddingCommand)
            {
                Console.Write("Обраний матеріал: ");
                cmdLine = UserInput.NotEmptyString(() => Console.ReadLine());
                int id = materialService.GetAll().ToList().Count + 1;
                switch (cmdLine)
                {
                    case Command.ArticleInputCase:
                        var article = ArticleUserInput(id);
                        _account.UserMaterials.Add(article);
                        materialService.Add(article);
                        courseMaterials.Add(article);
                        break;
                    case Command.PublicationInputCase:
                        var publication = PublicationUserInput(id);
                        _account.UserMaterials.Add(publication);
                        materialService.Add(publication);
                        courseMaterials.Add(publication);
                        break;
                    case Command.VideoInputCase:
                        var video = VideoUserInput(id);
                        _account.UserMaterials.Add(video);
                        materialService.Add(video);
                        courseMaterials.Add(video);
                        break;
                    case Command.StopAddingCommand:
                        Console.WriteLine("Матеріали додано");
                        break;
                    default:
                        Console.WriteLine("На жаль це не тип матеріалу\n" +
                                          "Натисніть Enter");
                        Console.ReadLine();
                        break;
                }
            }

            _userService.Save();
            return courseMaterials;
        }
        public IEnumerable<Material> AddExistingMaterials()
        {
            List<Material> newMaterials = new ();
            var userMaterials = _account.UserMaterials;
            Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
            userMaterials?.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

            var strMaterialsIds = UserInput.NotEmptyString(() => Console.ReadLine());
            var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
            listMaterialsIds.ForEach((stringMatId) =>
            {
                if (_validateService.Material.Validate(userMaterials, stringMatId, out Material material))
                {
                    newMaterials.Add(material);
                }
            });

            return newMaterials;
        }

        /// <inheritdoc/>
        public void AddSkill(Skill skill)
        {
            var skills = _account.UserSkills.ToList();
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            if (skills.Find(c => c.Name == skill.Name) != null)
            {
                var index = skills.IndexOf(skills.Find(c => c.Name == skill.Name));
                skills[index].Points += skill.Points;
            }
            else
            {
                skills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }

        public Skill CreateSkill(string cmdLine)
        {
            Skill newSkill;
            string[] skillStr = cmdLine?.Split(" = ");
            if (Enum.TryParse(skillStr[0], out SkillKind skillKind) && int.TryParse(skillStr[1], out int points))
            {
                if ((int)skillKind > 7 || (int)skillKind < 0)
                {
                    Console.WriteLine("Такої навички немає\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return null;
                }
           
                if (points <= 0)
                {
                    Console.WriteLine("Не можна вказувати стільки поінтів\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return null;
                }
           
                newSkill = new Skill { Name = skillKind, Points = points };
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Не вірний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return null;
            }
            

            return newSkill;
        }

        /// <inheritdoc/>
        public User Get() => _account;

        /// <inheritdoc/>
        public void Login(string email, string password)
        {
            var loginResult = TryLogin(email, password);
            if (loginResult)
            {
                Console.WriteLine($"З поверненням {_account.Name}\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Невірний email чи пароль");
                Console.ReadLine();
            }
        }

        /// <inheritdoc/>
        public void Logout()
        {
            _account = null;
        }

        /// <inheritdoc/>
        public void Register(string name, string email, string password)
        {
            TryRegister(name, email, password);
        }

        /// <inheritdoc/>
        public void RemoveCourse(int id)
        {
            var courses = _account.UserCourses;
            var pulledCourse = courses.FirstOrDefault(course => course.Key == id);
            if (pulledCourse.Value == null)
            {
                return;
            }

            courses.Remove(pulledCourse.Key);
        }

        /// <inheritdoc/>
        public void UpdateCourseInfo(Course editedCourse)
        {
            var courses = _account.UserCourses;
            if (editedCourse == null)
            {
                throw new ArgumentNullException(nameof(editedCourse));
            }

            var pulledUserCourse = courses.FirstOrDefault(course => course.Key == editedCourse.Id);
            if (pulledUserCourse.Value == null)
            {
                return;
            }

            courses.Remove(pulledUserCourse.Key);
            AddCourse(editedCourse);
        }

        private ArticleMaterial ArticleUserInput(int id)
        {
            Console.Write("Введіть назву статті: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть дату публікації статті: ");
            DateTime date = UserInput.ValidDateTime(() => Console.ReadLine());
            Console.Write("Введіть посиланя на статтю: ");
            string link = UserInput.NotEmptyString(() => Console.ReadLine());

            return new ArticleMaterial(id, title, date, link);
        }
        private PublicationMaterial PublicationUserInput(int id)
        {
            Console.Write("Введіть назву публікації: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть автора публікації: ");
            string author = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть кількість сторінок публікації: ");
            int pageCount = UserInput.ValidInt(() => Console.ReadLine());
            Console.Write("Введіть формат файлу публікації: ");
            string format = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть дату публікації: ");
            DateTime date = UserInput.ValidDateTime(() => Console.ReadLine());

            return new PublicationMaterial(id, title, author, pageCount, format, date);
        }

        private VideoMaterial VideoUserInput(int id)
        {
            Console.Write("Введіть назву відео: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть довжину відео: ");
            float duration = UserInput.ValidFloat(() => Console.ReadLine());
            Console.Write("Введіть якість відео: ");
            int quality = UserInput.ValidInt(() => Console.ReadLine());

            return new VideoMaterial(id, title, duration, quality);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                static string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool TryLogin(string email, string password)
        {
            var allUsers = _userService.GetAll();
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if (password == pulledUser.Password)
                {
                    _account = pulledUser;
                    return true;
                }
            }

            return false;
        }

        private void TryRegister(string name, string email, string password)
        {
            if (IsValidEmail(email))
            {
                var id = _userService.GetAll().ToList().Count + 1;
                var newUser = new User(id, name, email, password);
                _userService.Add(newUser);
                _account = newUser;
            }
            else
            {
                Console.WriteLine("E-mail у неправильному форматі\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
            }
        }
    }
}
