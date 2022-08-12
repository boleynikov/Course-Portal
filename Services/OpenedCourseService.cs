using Domain;
using Domain.CourseMaterials;
using Services.Helper;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    /// <summary>
    /// Opened Course functionallity
    /// </summary>
    public class OpenedCourseService : IOpenedCourseService
    {
        private readonly Course _course;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenedCourseService"/> class.
        /// </summary>
        /// <param name="course">Course, which will be opened</param>
        public OpenedCourseService(Course course)
        {
            _course = course;
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

        public void EditCourseName()
        {
            Console.Write("Введіть нову назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            _course.Name = name;
        }

        public void EditCourseDescription()
        {
            Console.Write("Введіть новий опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            _course.Description = description;
        }

        public void DeleteCourseMaterial()
        {
            Console.Write("Введіть ідентифікатор матеріалу: ");
            var currentCourse = _course;
            var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
            if (ValidateMaterial(strMaterialId, out Material material) && currentCourse.CourseMaterials.Contains(material))
            {
                currentCourse.CourseMaterials.Remove(material);
                Console.WriteLine($"Матеріал {strMaterialId} успішно видалено\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
            }
        }

        public bool ValidateMaterial(string strMaterialId, out Material material, List<Material> userMaterials = null)
        {
            List<Material> materials;
            if (userMaterials != null)
            {
                materials = userMaterials;
            }
            else
            {
                materials = _course.CourseMaterials;
            }

            if (int.TryParse(strMaterialId, out int materialId))
            {
                try
                {
                    material = materials.FirstOrDefault(c => c.Id == materialId)
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
                if (ValidateMaterial(stringMatId, out Material material, userMaterials) && !currentCourse.CourseMaterials.Contains(material))
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
