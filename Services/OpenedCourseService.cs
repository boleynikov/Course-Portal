using Domain;
using Domain.CourseMaterials;
using Services.Helper;
using Services.Interface;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enum;

namespace Services
{
    /// <summary>
    /// Opened Course functionality
    /// </summary>
    public class OpenedCourseService : IOpenedCourseService
    {
        private readonly Course _currentCourse;
        private readonly Validator _validateService;
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenedCourseService"/> class.
        /// </summary>
        /// <param name="currentCourse">Course, which will be opened</param>
        public OpenedCourseService(Course currentCourse, Validator validateService)
        {
            _currentCourse = currentCourse;
            _validateService = validateService;
        }

        /// <inheritdoc/>
        public Course Get() => _currentCourse;

        /// <inheritdoc/>
        public void AddOrEditSkill()
        {
            string cmdLine = string.Empty;
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

            Skill skill = AuthorizedUserService.CreateSkill(cmdLine);
            if (skill == null)
            {
                return;
            }

            var skills = _currentCourse.CourseSkills;
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

            _currentCourse.Status = CourseStatus.InEditing;
        }
        /// <inheritdoc/>
        public void DeleteSkill()
        {
            Console.Write("Введіть назву навички, яку хочете видалити з курсу: ");
            string skillName = UserInput.NotEmptyString(() => Console.ReadLine());
            if (Enum.TryParse(skillName, out SkillKind name))
            {
                var skill = _currentCourse.CourseSkills.ToList().Find(s => s.Name == name);
                _currentCourse.CourseSkills.Remove(skill);
                _currentCourse.Status = CourseStatus.InEditing;
            }
        }
        /// <inheritdoc/>
        public void EditCourseName()
        {
            Console.Write("Введіть нову назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            _currentCourse.Name = name;
            _currentCourse.Status = CourseStatus.InEditing;
        }
        /// <inheritdoc/>
        public void EditCourseDescription()
        {
            Console.Write("Введіть новий опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            _currentCourse.Description = description;
            _currentCourse.Status = CourseStatus.InEditing;
        }
        /// <inheritdoc/>
        public int DeleteCourseMaterial()
        {
            Console.Write("Введіть ідентифікатор матеріалу: ");
            var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
            if (_validateService.Material.Validate(_currentCourse.CourseMaterials.ToList(), strMaterialId, out Material material) && _currentCourse.CourseMaterials.Contains(material))
            {
                _currentCourse.CourseMaterials.Remove(material);
                _currentCourse.Status = CourseStatus.InEditing;
                Console.WriteLine($"Матеріал {strMaterialId} успішно видалено\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
                return material.Id;
            }

            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc/>
        public void AddCourseMaterial(List<Material> userMaterials)
        {
            if (userMaterials == null)
            {
                throw new ArgumentNullException(nameof(userMaterials));
            }

            Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
            userMaterials.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

            var strMaterialsIds = UserInput.NotEmptyString(() => Console.ReadLine());
            var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
            listMaterialsIds.ForEach((stringMatId) =>
            {
                if (_validateService.Material.Validate(userMaterials, stringMatId, out Material material) && !_currentCourse.CourseMaterials.Contains(material))
                {
                    _currentCourse.CourseMaterials.Add(material);
                    _currentCourse.Status = CourseStatus.InEditing;
                }
                else
                {
                    Console.WriteLine($"Матеріал з id {stringMatId} вже є у матеріалах курсу\n" +
                                           "Натисніть Enter");
                    Console.ReadLine();
                }
            });
        }
    }
}
