using Domain;
using Domain.CourseMaterials;
using Services.Helper;
using Services.Interface;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    /// <summary>
    /// Opened Course functionality
    /// </summary>
    public class OpenedCourseService : IOpenedCourseService
    {
        private readonly Course _course;
        private readonly Validator _validateService;
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenedCourseService"/> class.
        /// </summary>
        /// <param name="course">Course, which will be opened</param>
        public OpenedCourseService(Course course, Validator validateService)
        {
            _course = course;
            _validateService = validateService;
        }

        /// <inheritdoc/>
        public Course Get() => _course;

        /// <inheritdoc/>
        public void AddSkill(Course currentCourse, Skill skill, int value)
        {
            var skills = _course.CourseSkills;
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            var skillExist = skills.Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = skills.IndexOf(skillExist);
                skills[index].Points += value;
            }
            else
            {
                skills.Add(new Skill { Name = skill.Name, Points = value });
            }
        }
        /// <inheritdoc/>
        public void EditCourseName()
        {
            Console.Write("Введіть нову назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            _course.Name = name;
        }
        /// <inheritdoc/>
        public void EditCourseDescription()
        {
            Console.Write("Введіть новий опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            _course.Description = description;
        }
        /// <inheritdoc/>
        public int DeleteCourseMaterial()
        {
            Console.Write("Введіть ідентифікатор матеріалу: ");
            var currentCourse = _course;
            var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
            if (_validateService.Material.Validate(currentCourse.CourseMaterials, strMaterialId, out Material material) && currentCourse.CourseMaterials.Contains(material))
            {
                currentCourse.CourseMaterials.Remove(material);
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
            var currentCourse = _course;
            Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
            userMaterials.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

            var strMaterialsIds = UserInput.NotEmptyString(() => Console.ReadLine());
            var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
            listMaterialsIds.ForEach((stringMatId) =>
            {
                if (_validateService.Material.Validate(userMaterials, stringMatId, out Material material) && !currentCourse.CourseMaterials.Contains(material))
                {
                    currentCourse.CourseMaterials.Add(material);
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
