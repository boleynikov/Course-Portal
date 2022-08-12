// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API.Controllers.Abstract;
    using API.Controllers.Helper;
    using API.View;
    using Domain;
    using Domain.CourseMaterials;
    using Services.Interface;

    /// <summary>
    /// Course Controller.
    /// </summary>
    public class CourseController : IController
    {
        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;
        private readonly IAuthorizationService _authorizedUser;
        private readonly IOpenedCourseService _openedCourse;
        private readonly string _redirectPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="userService">User service instance.</param>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="authorizedUser">Current authorized user service</param>
        /// <param name="course">Course, that displayed.</param>
        /// <param name="redirectPage">String of page for redirect back.</param>
        public CourseController(
            IService<User> userService,
            IService<Course> courseService,
            IAuthorizationService authorizedUser,
            IOpenedCourseService openedCourse,
            string redirectPage = "home")
        {
            _userService = userService;
            _courseService = courseService;
            _openedCourse = openedCourse;
            _authorizedUser = authorizedUser;
            _redirectPage = redirectPage;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            string page = Command.CoursePage;
            while (page == Command.CoursePage)
            {
                Console.Clear();
                var currentUser = _authorizedUser.Get();
                var currentCourse = _openedCourse.Get();
                CoursePageView.Show(currentUser, currentCourse);
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case Command.AddCourseCommand:
                        _authorizedUser.AddCourse(currentCourse);
                        _userService.Save();
                        break;
                    case Command.EditCommand:
                        EditCourse();
                        _authorizedUser.UpdateCourseInfo(currentCourse);
                        _courseService.Update(currentCourse);
                        _userService.Update(currentUser);
                        break;
                    case Command.BackCommand:
                        page = _redirectPage;
                        break;
                    default:
                        Console.WriteLine("Команду не розпізнано\n" +
                                  "Натисніть Enter");
                        Console.ReadLine();
                        break;
                }
            }

            return page;
        }

        private void EditCourse()
        {
            var currentCourse = _openedCourse.Get();
            var name = currentCourse.Name;
            var description = currentCourse.Description;
            CoursePageView.EditNavigationView();
            var str = UserInput.NotEmptyString(() => Console.ReadLine());
            string[] editCmd = str.Split(", ");
            foreach (var cmd in editCmd)
            {
                switch (cmd)
                {
                    case Command.EditCourseName:
                        EditCourseName();
                        break;
                    case Command.EditCourseDescription:
                        EditCourseDescription();
                        break;
                    case Command.DeleteCourseMaterial:
                        DeleteCourseMaterial();
                        break;
                    case Command.AddCourseMaterials:
                        AddCourseMaterial();
                        break;
                }
            }
        }

        private void EditCourseName()
        {
            Console.Write("Введіть нову назву курсу: ");
            string name = UserInput.NotEmptyString(() => Console.ReadLine());
            _openedCourse.Get().Name = name;
        }

        private void EditCourseDescription()
        {
            Console.Write("Введіть новий опис курсу: ");
            string description = UserInput.NotEmptyString(() => Console.ReadLine());
            _openedCourse.Get().Description = description;
        }

        private void DeleteCourseMaterial()
        {
            Console.Write("Введіть ідентифікатор матеріалу: ");
            var currentCourse = _openedCourse.Get();
            var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
            if (ValidateMaterial(strMaterialId, out Material material) && currentCourse.CourseMaterials.Contains(material))
            {
                currentCourse.CourseMaterials.Remove(material);
                Console.WriteLine($"Матеріал {strMaterialId} успішно видалено\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
            }
        }

        private void AddCourseMaterial()
        {
            var currentCourse = _openedCourse.Get();
            var userMaterials = _authorizedUser.Get().UserMaterials.ToList();
            Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
            userMaterials.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

            var strMaterialsIds = UserInput.NotEmptyString(() => Console.ReadLine());
            var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
            listMaterialsIds.ForEach((stringMatId) =>
            {
                if (ValidateMaterial(stringMatId, out Material material, "user") && !currentCourse.CourseMaterials.Contains(material))
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

        private bool ValidateMaterial(string strMaterialId, out Material material, string materialAttachedTo = "course")
        {
            List<Material> materials;
            if (materialAttachedTo == "user")
            {
                materials = _authorizedUser.Get().UserMaterials.ToList();
            }
            else
            {
                materials = _openedCourse.Get().CourseMaterials;
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
    }
}
