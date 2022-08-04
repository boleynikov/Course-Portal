using API.Controllers.Abstract;
using Domain;
using Domain.CourseMaterials;
using Services.Abstract;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    public class UserController : IController
    {
        private readonly IService<Course> courseService;
        private readonly IService<Material> materialService;
        private readonly IService<User> userService;
        private readonly int userId;
        public UserController(IService<Course> courseService, IService<Material> materialService, IService<User> userService, int userId)
        {
            this.courseService = courseService;
            this.materialService = materialService;
            this.userService = userService;
            this.userId = userId;
        }

        public string Launch()
        {            
            return View();
        }

        private string View()
        {
            string page = "user";
            while (page == "user")
            {
                Console.Clear();
                var currentUser = userService.GetByIndex(userId);
                Console.WriteLine($"Обліковий запис");
                Console.WriteLine($"Ім'я: {currentUser.Name}");
                Console.WriteLine($"Email: {currentUser.Email}");
                if (currentUser.UserCourses.Count <= 0)
                {
                    Console.WriteLine("У вас ще немає доданих чи створених курсів.");
                }
                else
                {
                    Console.WriteLine($"Кількість курсів користувача: {currentUser.UserCourses.Count}");
                    Console.WriteLine("Список наявних курсів:");
                    foreach (var courseKeyValue in currentUser.UserCourses)
                    {
                        var course = courseKeyValue.Item1;
                        var progress = courseKeyValue.Item2;
                        Console.WriteLine($"{course.Id}. {course.Name} - Прогресс: {progress.State}, {progress.Percentage} %");
                    }
                }

                Console.WriteLine("Щоб створити курс - введіть \"create\"");
                Console.WriteLine("Щоб видалити курс - введіть \"delete\"");
                Console.WriteLine("Щоб повернутися назад - введіть \"home\"");

                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "create":
                        CreateCourse();
                        break;
                    case "delete":
                        Console.Write("Введіть номер курсу: ");
                        int courseId = int.Parse(Console.ReadLine());
                        currentUser.RemoveCourse(courseId);
                        userService.Save();
                        break;
                    case "home":
                        page = "home";
                        break;
                }
            }
            return page;
        }

        private void CreateCourse()
        {
            Console.Clear();

            Console.Write("Введіть назву курсу: ");
            string name = Console.ReadLine();

            Console.Write("Введіть опис курсу: ");
            string description = Console.ReadLine();

            int id = courseService.GetAll().Length + 1;
            var course = new Course(id, name, description);

            var materials = CreateMaterials();
            var skills = CreateSkills();

            foreach(var material in materials)
            {
                course.AddMaterial(material);
            }

            foreach (var skill in skills)
            {
                course.AddSkill(skill, skill.Points);
            }

            userService.GetByIndex(userId).AddCourse(course);
            userService.Save();
            courseService.Add(course);
            Console.Write("Курс успішно додано. Натисніть Enter");
            Console.ReadLine();
        }

        private List<Skill> CreateSkills()
        {
            List<Skill> courseSkills = new List<Skill>();
            Console.WriteLine("Оберіть навички, які можна отримати пройшовши курс:");
            string cmdLine = String.Empty;
            while (cmdLine != "stop")
            {
                Console.WriteLine("Доступні навички:");
                Console.WriteLine("Programming,\nMusic,\nHealthCare,\nTimeManagment,\nCommunication,\nIllustration,\nPhoto");
                Console.WriteLine("Введіть назву навика і кількість поінтів через дорівнює (Ось так: \"Programming = 3\")");
                Console.WriteLine("Або введіть \"stop\", щоб зупинитися");
                cmdLine = Console.ReadLine();
                if (cmdLine == "stop")
                    break;
                string[] skillStr = cmdLine.Split(" = ");
                if (Enum.TryParse(skillStr[0], out SkillKind skillKind) && int.TryParse(skillStr[1], out int points))
                {
                    var newSkill = new Skill { Name = skillKind, Points = points };
                    courseSkills.Add(newSkill);
                }
            }
            return courseSkills;
        }
        private List<Material> CreateMaterials()
        {
            List<Material> courseMaterials = new List<Material>();
            string cmdLine = string.Empty;
            while(cmdLine!= "stop")
            {
                Console.WriteLine("Введіть тип матеріалу, який хочете додати до курсу");
                Console.WriteLine("Доступні матеріали: Article, Publication, Video");
                Console.WriteLine("Або введіть \"stop\", щоб зупинитися");
                Console.Write("Обраний матеріал: ");
                cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case "Article":
                        Console.Write("Введіть назву статті:");
                        string title = Console.ReadLine();
                        Console.Write("Введіть дату публікації статті: ");
                        DateTime.TryParse(Console.ReadLine(), out DateTime date);
                        Console.Write("Введіть посиланя на статтю: ");
                        string link = Console.ReadLine();

                        int id = materialService.GetAll().Length + 1;
                        var articleMaterial = new ArticleMaterial(id, title, date, link);

                        userService.GetByIndex(userId).AddMaterial(articleMaterial);
                        materialService.Add(articleMaterial);
                        courseMaterials.Add(articleMaterial);
                        break;
                    case "Publication":
                        Console.Write("Введіть назву публікації:");
                        title = Console.ReadLine();
                        Console.Write("Введіть автора публікації: ");
                        string author = Console.ReadLine();
                        Console.Write("Введіть кількість сторінок публікації: ");
                        int pageCount = int.Parse(Console.ReadLine());
                        Console.Write("Введіть формат файлу публікації:");
                        string format = Console.ReadLine();
                        Console.Write("Введіть дату публікації:");
                        DateTime.TryParse(Console.ReadLine(), out date);

                        id = materialService.GetAll().Length + 1;
                        var publicationMaterial = new PublicationMaterial(id, title, author, pageCount, format, date);

                        userService.GetByIndex(userId).AddMaterial(publicationMaterial);
                        materialService.Add(publicationMaterial);
                        courseMaterials.Add(publicationMaterial);
                        break;
                    case "Video":
                        Console.Write("Введіть назву відео:");
                        title = Console.ReadLine();
                        Console.Write("Введіть довжину відео: ");
                        float.TryParse(Console.ReadLine(), out float duration);
                        Console.Write("Введіть якість відео: ");
                        int quality = int.Parse(Console.ReadLine());

                        id = materialService.GetAll().Length + 1;
                        var videoMaterial = new VideoMaterial(id, title, duration, quality);

                        userService.GetByIndex(userId).AddMaterial(videoMaterial);
                        materialService.Add(videoMaterial);
                        courseMaterials.Add(videoMaterial);
                        break;
                }
            }

            userService.Save();
            return courseMaterials;
        }
    }
}
