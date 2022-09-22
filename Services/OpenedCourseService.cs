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
        public void AddOrEditSkill(Course course, string skillName, int skillPoint)
        {
            var skill = new Skill() { Name = Enum.Parse<SkillKind>(skillName), Points = skillPoint };
            var existingSkill = course?.CourseSkills.ToList().Find(c => c.Name == skill.Name);

            if (existingSkill != null)
            {
                var index = course.CourseSkills.ToList().IndexOf(existingSkill);
                course.CourseSkills.ElementAt(index).Points += skill.Points;
            }
            else
            {
                course.CourseSkills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }

            course.Status = CourseStatus.Edited;
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
                _currentCourse.Status = CourseStatus.Edited;
            }
        }
        /// <inheritdoc/>
        public void EditCourseName()
        {
            Console.Write("Введіть нову назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            _currentCourse.Name = name;
            _currentCourse.Status = CourseStatus.Edited;
        }
        /// <inheritdoc/>
        public void EditCourseDescription()
        {
            Console.Write("Введіть новий опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            _currentCourse.Description = description;
            _currentCourse.Status = CourseStatus.Edited;
        }
        /// <inheritdoc/>
        public int DeleteCourseMaterial(int id)
        {
            var material = _currentCourse.CourseMaterials.FirstOrDefault(m => m.Id == id);
            if (material == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            _currentCourse.CourseMaterials.Remove(material);
            _currentCourse.Status = CourseStatus.Edited;
            return material.Id;
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
                    _currentCourse.Status = CourseStatus.Edited;
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
