using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.CourseMaterials;
using Domain.Enum;
using Services.Helper;
using Services.Interface;
using Services.Validators;

namespace Services
{
    public class AuthorizedUserService : IAuthorizedUserService
    {
        private readonly IService<User> _userService;
        private readonly Validator _validateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedUserService"/> class.
        /// </summary>
        /// <param name="service">User service for decorating.</param>
        /// <param name="validateService">Validation service for input new courses & materials</param>
        /// <param name="account">Current authorized user.</param>
        public AuthorizedUserService(IService<User> service, Validator validateService)
        {
            _userService = service;
            _validateService = validateService;
        }

        public User Account { get; set; }

        public void AddCourse(Course newCourse)
        {
            if (newCourse == null)
            {
                throw new ArgumentNullException(nameof(newCourse));
            }

            if (Account.UserCourses.FirstOrDefault(c => c.Key == newCourse.Id).Key != 0)
            {
                Console.WriteLine("Даний курс у вас уже є\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return;
            }

            var item = new KeyValuePair<int, CourseProgress>(newCourse.Id, new CourseProgress() { State = State.NotCompleted, Percentage = 0f });
            Account.UserCourses.Add(item.Key, item.Value);
            _userService.Update(Account);
        }

        public IEnumerable<Material> AddExistingMaterials()
        {
            List<Material> newMaterials = new();
            var userMaterials = Account.UserMaterials.ToList();
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

        public void AddSkill(Skill skill)
        {
            var skills = Account.UserSkills.ToList();
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
            int coursesCount = courseService.GetAll().Count();
            int id;
            if (coursesCount == 0)
            {
                id = 1;
            }
            else
            {
                id = courseService.GetAll().ToList()[coursesCount - 1].Id + 1;
            }
            var course = new Course(id, name, description);
            List<Material> materials = new();
            if (Account.UserMaterials.ToList().Count > 0)
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
                int materialsCount = materialService.GetAll().Count();
                int id;
                if (materialsCount == 0)
                {
                    id = 1;
                }
                else
                {
                    id = materialService.GetAll().ToList()[materialsCount - 1].Id + 1;
                }

                switch (cmdLine)
                {
                    case Command.ArticleInputCase:
                        var article = ArticleUserInput(id);
                        Account.UserMaterials.Add(article);
                        materialService.Add(article);
                        courseMaterials.Add(article);
                        break;
                    case Command.PublicationInputCase:
                        var publication = PublicationUserInput(id);
                        Account.UserMaterials.Add(publication);
                        materialService.Add(publication);
                        courseMaterials.Add(publication);
                        break;
                    case Command.VideoInputCase:
                        var video = VideoUserInput(id);
                        Account.UserMaterials.Add(video);
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

        public static Skill CreateSkill(string cmdLine)
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

        public void RemoveCourse(int id)
        {
            var courses = Account.UserCourses;
            var pulledCourse = courses.FirstOrDefault(course => course.Key == id);
            if (pulledCourse.Value == null)
            {
                return;
            }

            courses.Remove(pulledCourse.Key);
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
    }
}
