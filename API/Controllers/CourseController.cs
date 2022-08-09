// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers
{
    using System;
    using System.Linq;
    using API.Controllers.Abstract;
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
        private const string _editCourseMaterials = "3";

        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;
        private readonly Course _course;
        private readonly int _userId;
        private readonly string _redirectPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="userService">User service instance.</param>
        /// <param name="courseService">Course service instance.</param>
        /// <param name="userId">Id of current user.</param>
        /// <param name="course">Course, that displayed.</param>
        /// <param name="redirectPage">String of page for redirect back.</param>
        public CourseController(
            IService<User> userService,
            IService<Course> courseService,
            int userId,
            Course course,
            string redirectPage = "home")
        {
            _userService = userService;
            _courseService = courseService;
            _course = course;
            _userId = userId;
            _redirectPage = redirectPage;
        }

        /// <inheritdoc/>
        public string Launch()
        {
            string page = _coursePage;
            while (page == _coursePage)
            {
                Console.Clear();
                var currentUser = _userService.GetByIndex(_userId);
                Console.WriteLine($"Курс: {_course.Name}");
                var pulledCoursePair = currentUser.UserCourses.Find(c => c.Item1.Id == _course.Id);
                if (pulledCoursePair.Item1 != null)
                {
                    Console.WriteLine($"\tВаш прогрес: {pulledCoursePair.Item2.State} {pulledCoursePair.Item2.Percentage} %");
                }

                Console.WriteLine($"Опис: {_course.Description}\n" +
                                   "Матеріали курсу:");
                foreach (var material in _course.CourseMaterials)
                {
                    Console.WriteLine("\t{0,20} | {1,5}", material.Type, material.Title);
                }

                Console.WriteLine("Навички, які ви отримаєте при проходженні курсу:");
                foreach (var skill in _course.CourseSkills)
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
                        currentUser.AddCourse(_course);
                        _userService.Save();
                        break;
                    case _editCommand:
                        EditCourse();
                        currentUser.UpdateCourseInfo(_course);
                        _courseService.Update(_course);
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
            var name = _course.Name;
            var description = _course.Description;
            Console.WriteLine("Введіть цифри у відповідності до того що саме ви хочете відредагувати\n" +
                              "через кому пробіл [, ]\n" +
                              $"{_editCourseName} - змінити назву\n" +
                              $"{_editCourseDescription} - змінити опис\n" +
                              $"{_editCourseMaterials} - додати матеріали із вже завантажених користувачем");

            var str = InputNotEmptyString(Console.ReadLine());
            string[] editCmd = str.Split(", ");
            foreach (var cmd in editCmd)
            {
                switch (cmd)
                {
                    case _editCourseName:
                        Console.Write("Введіть нову назву курсу: ");
                        name = InputNotEmptyString(Console.ReadLine());
                        break;
                    case _editCourseDescription:
                        Console.Write("Введіть новий опис курсу: ");
                        description = InputNotEmptyString(Console.ReadLine());
                        break;
                    case _editCourseMaterials:
                        var userMaterials = _userService.GetByIndex(_userId).UserMaterials;
                        Console.WriteLine("Оберіть номери матеріалів, які ви хочете додати через кому з пробілом [, ]");
                        userMaterials.ForEach((mat) => Console.WriteLine($"{mat.Id} {mat.Title}"));

                        var strMaterialsIds = InputNotEmptyString(Console.ReadLine());
                        var listMaterialsIds = strMaterialsIds.Split(", ").ToList();
                        listMaterialsIds.ForEach((strMatId) =>
                        {
                            if (ValidateMaterial(strMatId, out Material material))
                            {
                                _course.CourseMaterials.Add(material);
                            }
                        });
                        break;
                }
            }

            _course.UpdateInfo(name, description, _course.CourseMaterials, _course.CourseSkills);
        }

        private bool ValidateMaterial(string strMaterialId, out Material material)
        {
            if (int.TryParse(strMaterialId, out int materialId))
            {
                try
                {
                    material = _courseService.GetByIndex(_userId).CourseMaterials.FirstOrDefault(c => c.Id == materialId)
                        ?? throw new ArgumentOutOfRangeException(nameof(materialId));
                    if (_course.CourseMaterials.Contains(material))
                    {
                        Console.WriteLine($"Матеріал з id {materialId} вже є у матеріалах курсу\n" +
                                           "Натисніть Enter");
                        Console.ReadLine();
                        return false;
                    }

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

        private string InputNotEmptyString(string inputString)
        {
            while (string.IsNullOrWhiteSpace(inputString))
            {
                if (string.IsNullOrWhiteSpace(inputString))
                {
                    Console.Write("Ви ввели порожню строку.Спробуйте ще раз: ");
                }

                inputString = Console.ReadLine();
            }

            return inputString;
        }
    }
}
