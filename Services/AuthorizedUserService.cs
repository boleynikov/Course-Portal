using System;
using System.Collections.Generic;
using System.Linq;
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
        public AuthorizedUserService(IService<User> service, Validator validateService)
        {
            _userService = service;
            _validateService = validateService;
        }

        public User Account { get; set; }

        public async Task AddCourseToUser(int courseId)
        {
            var item = new KeyValuePair<int, CourseProgress>(courseId, new CourseProgress() { State = State.NotCompleted, Percentage = 0f });
            Account.UserCourses.Add(item.Key, item.Value);
            await _userService.Update(Account);
        }

        public void EditCourseProgress(int courseId, float percentage)
        {
            var key = Account.UserCourses.FirstOrDefault(c => c.Key == courseId).Key;
            if (Account.UserCourses[key].State == State.Completed)
            {
                return;
            }

            Account.UserCourses[key].Percentage += percentage;
            if (Account.UserCourses[key].Percentage >= 100f)
            {
                Account.UserCourses[key].State = State.PreCompleted;
            }
        }
        public void RemoveCourse(int id)
        {
            var pulledCourse = Account.UserCourses.FirstOrDefault(course => course.Key == id);
            if (pulledCourse.Value == null)
            {
                return;
            }

            Account.UserCourses.Remove(pulledCourse.Key);
        }

        public void AddSkill(Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            var skills = Account.UserSkills;
            var existingSkill = skills.ToList().Find(c => c.Name == skill.Name);
            
            if (existingSkill != null)
            {
                var index = skills.ToList().IndexOf(existingSkill);
                skills.ElementAt(index).Points += skill.Points;
            }
            else
            {
                skills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }

        public async Task<Course> CreateCourse(string name, string description, IService<Course> courseService, IService<Material> materialService)
        {
            if (courseService == null)
            {
                throw new ArgumentNullException(nameof(courseService));
            }

            var allCourses = await courseService.GetAll(0);
            int coursesCount = allCourses.Count();
            int id;
            if (coursesCount == 0)
            {
                id = 1;
            }
            else
            {
                id = allCourses.ToList()[coursesCount - 1].Id + 1;
            }

            var course = new Course(id, name, description);
            return course;
        }

        public async Task<Material> AddMaterial(IService<Material> materialService, Material material)
        {
            if (materialService == null)
            {
                throw new ArgumentNullException(nameof(materialService));
            }
            if (material == null)
            {
                throw new ArgumentNullException(nameof(material));
            }

            var allMaterials = await materialService.GetAll(0);
            int materialsCount = allMaterials.Count();
            if (materialsCount == 0)
            {
                material.Id = 1;
            }
            else
            {
                material.Id = allMaterials.ToList()[materialsCount - 1].Id + 1;
            }

            Account.UserMaterials.Add(material);
            await materialService.Add(material);
            

            await _userService.Save();
            return material;
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
        public static Skill CreateSkill(string cmdLine)
        {
            Skill newSkill;
            string[] skillStr = cmdLine?.Split(" = ");

            if (skillStr.Length > 1 && Enum.TryParse(skillStr[0], out SkillKind skillKind) && int.TryParse(skillStr[1], out int points))
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
