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
    using Domain;
    using Domain.CourseMaterials;
    using Services.Interface;

    /// <summary>
    /// Course Controller.
    /// </summary>
    public class CourseController : IController
    {
        private const string _coursePage = "course";
        private const string _addCommand = "0";
        private const string _editCommand = "1";
        private const string _exitCommand = "2";

        private const string _editCourseName = "1";
        private const string _editCourseDescription = "2";
        private const string _addCourseMaterials = "3";
        private const string _deleteCourseMaterial = "4";

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
            string page = _coursePage;
            while (page == _coursePage)
            {
                Console.Clear();
                var currentUser = _authorizedUser.GetCurrentAccount();
                var currentCourse = _openedCourse.Get();
                Console.WriteLine($"Курс: {currentCourse.Name}");
                var pulledCourseTuple = currentUser.UserCourses.Find(c => c.Course.Id == currentCourse.Id);
                if (pulledCourseTuple.Course != null)
                {
                    Console.WriteLine($"\tВаш прогрес: {pulledCourseTuple.Progress.State} {pulledCourseTuple.Progress.Percentage} %");
                }

                Console.WriteLine($"Опис: {currentCourse.Description}\n" +
                                   "Матеріали курсу:");
                foreach (var material in currentCourse.CourseMaterials)
                {
                    Console.WriteLine("\t{0, 2} {1,20} | {2,5}", material.Id, material.Type, material.Title);
                }

                Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
                foreach (var skill in currentCourse.CourseSkills)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", skill.Name, skill.Points);
                }

                Console.WriteLine($"Щоб додати до свого списку курс - введіть {_addCommand}\n" +
                                  $"Щоб змінити назву опис чи додати матеріали до курсу - введіть {_editCommand}\n" +
                                  $"Щоб повернутися назад - введіть {_exitCommand}\n");
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case _addCommand:
                        _authorizedUser.AddCourse(currentCourse);
                        _userService.Save();
                        break;
                    case _editCommand:
                        EditCourse();
                        _authorizedUser.UpdateCourseInfo(currentCourse);
                        _courseService.Update(currentCourse);
                        _userService.Update(currentUser);
                        break;
                    case _exitCommand:
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
            Console.WriteLine("Введіть цифри у відповідності до того що саме ви хочете відредагувати\n" +
                              "через кому пробіл [, ]\n" +
                              $"{_editCourseName} - змінити назву\n" +
                              $"{_editCourseDescription} - змінити опис\n" +
                              $"{_addCourseMaterials} - додати матеріали із вже завантажених користувачем\n" +
                              $"{_deleteCourseMaterial} - видалити матеріал із курсу");

            var str = UserInput.NotEmptyString(() => Console.ReadLine());
            string[] editCmd = str.Split(", ");
            foreach (var cmd in editCmd)
            {
                switch (cmd)
                {
                    case _editCourseName:
                        Console.Write("Введіть нову назву курсу: ");
                        name = UserInput.NotEmptyString(() => Console.ReadLine());
                        currentCourse.Name = name;
                        break;
                    case _editCourseDescription:
                        Console.Write("Введіть новий опис курсу: ");
                        description = UserInput.NotEmptyString(() => Console.ReadLine());
                        currentCourse.Description = description;
                        break;
                    case _deleteCourseMaterial:
                        Console.Write("Введіть ідентифікатор матеріалу: ");
                        var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
                        if (ValidateMaterial(strMaterialId, out Material material) && currentCourse.CourseMaterials.Contains(material))
                        {
                            currentCourse.CourseMaterials.Remove(material);
                            Console.WriteLine($"Матеріал {strMaterialId} успішно видалено\n" +
                                               "Натисніть Enter");
                            Console.ReadLine();
                        }

                        break;
                    case _addCourseMaterials:
                        var userMaterials = _authorizedUser.GetCurrentAccount().UserMaterials;
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
                        break;
                }
            }
        }

        private bool ValidateMaterial(string strMaterialId, out Material material, string materialAttachedTo = "course")
        {
            List<Material> materials;
            if (materialAttachedTo == "user")
            {
                materials = _authorizedUser.GetCurrentAccount().UserMaterials;
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
