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
        public void AddSkill(Course currentCourse, Skill skill, int value)
        {
            var skills = _currentCourse.CourseSkills;
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            var skillExist = skills.ToList().Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = skills.ToList().IndexOf(skillExist);
                skills.ElementAt(index).Points += value;
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
