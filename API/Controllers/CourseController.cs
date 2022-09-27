// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Domain.Enum;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    using Abstract;
    using Domain;
    using Services.Helper;
    using Services.Interface;
    using System;
    using View;

    /// <summary>
    /// Course Controller.
    /// </summary>
    public class CourseController : IController
    {
        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;

        private readonly IAuthorizedUserService _authorizedUser;
        private readonly IOpenedCourseService _openedCourse;
        private readonly string _redirectPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="userService">User service instance</param>
        /// <param name="courseService">Course service instance</param>
        /// <param name="openedCourse">Current opened course service</param>
        /// <param name="authorizedUser">Current authorized user service</param>
        /// <param name="redirectPage">String of page for redirect back</param>
        public CourseController(
            IService<User> userService,
            IService<Course> courseService,
            IAuthorizedUserService authorizedUser,
            IOpenedCourseService openedCourse,
            string redirectPage = "home")
        {
            _userService = userService;
            _courseService = courseService;
            _openedCourse = openedCourse;
            _authorizedUser = authorizedUser;
            _redirectPage = redirectPage;
        }

        /// <summary>
        /// Checks course status
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static bool IsCourseNotDeleted(Course course)
        {
            if (course?.Status == CourseStatus.Deleted)
            {
                Console.WriteLine("Нажаль цей курс видалено");
                Console.ReadLine();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<string> Launch()
        {
            var page = Command.CoursePage;
            while (page == Command.CoursePage)
            {
                Console.Clear();
                var currentUser = _authorizedUser.Account;
                var currentCourse = _openedCourse.Get();
                CoursePageView.Show(currentUser, currentCourse);
                var cmdLine = Console.ReadLine();
                if (currentUser.UserCourses.FirstOrDefault(c => c.Key == currentCourse.Id).Key != 0)
                {
                    switch (cmdLine)
                    {
                        case Command.OpenCourseCommand:
                            if (!IsCourseNotDeleted(currentCourse))
                            {
                                break;
                            }

                            page = await new MaterialController(_authorizedUser, currentCourse).Launch();
                            if (currentUser.UserCourses[_openedCourse.Get().Id].State == State.PreCompleted)
                            {
                                foreach (var courseSkill in _openedCourse.Get().CourseSkills)
                                {
                                    _authorizedUser.AddSkill(courseSkill);
                                }

                                currentUser.UserCourses[_openedCourse.Get().Id].State = State.Completed;
                            }

                            await _userService.Update(currentUser);
                            break;
                        case Command.EditCommand:
                            if (!IsCourseNotDeleted(currentCourse))
                            {
                                break;
                            }

                            EditCourse();
                            await _courseService.Update(currentCourse);
                            await _userService.Update(currentUser);
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
                else
                {
                    switch (cmdLine)
                    {
                        case Command.AddCourseCommand:
                            if (!IsCourseNotDeleted(currentCourse))
                            {
                                break;
                            }

                            await _authorizedUser.AddCourseToUser(currentCourse.Id);
                            await _userService.Save();
                            break;
                        case Command.EditCommand:
                            if (!IsCourseNotDeleted(currentCourse))
                            {
                                break;
                            }

                            EditCourse();
                            await _courseService.Update(currentCourse);
                            await _userService.Update(currentUser);
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
            }

            return page;
        }

        private void EditCourse()
        {
            CoursePageView.EditNavigationView();
            var str = UserInput.NotEmptyString(() => Console.ReadLine());
            var editCmd = str.Split(", ");
            foreach (var cmd in editCmd)
            {
                switch (cmd)
                {
                    case Command.EditCourseName:
                        _openedCourse.EditCourseName();
                        break;
                    case Command.EditCourseDescription:
                        _openedCourse.EditCourseDescription();
                        break;
                    case Command.DeleteCourseMaterial:
                        try
                        {
                            Console.Write("Введіть ідентифікатор матеріалу: ");
                            var strMaterialId = UserInput.NotEmptyString(() => Console.ReadLine());
                            var materialId = int.Parse(strMaterialId);
                            _openedCourse.DeleteCourseMaterial(materialId);
                        }
                        catch
                        {
                            Console.WriteLine("Такого ідентифікатору немає");
                            Console.ReadLine();
                        }

                        break;
                    case Command.AddCourseMaterials:
                        _openedCourse.AddCourseMaterial(_authorizedUser.Account.UserMaterials.ToList());
                        break;
                    case Command.AddNewOrEditSkill:
                        var skill = CreateSkill();
                        if (skill != null)
                        {
                            _openedCourse.AddOrEditSkill(skill.Name, skill.Points);
                        }

                        break;
                    case Command.DeleteSkill:
                        try
                        {
                            Console.Write("Введіть назву навички, яку хочете видалити з курсу: ");
                            string skillName = UserInput.NotEmptyString(() => Console.ReadLine());
                            _openedCourse.DeleteSkill(skillName);
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("Такої навички немає");
                            Console.ReadLine();
                        }

                        break;
                }
            }
        }

        private Skill CreateSkill()
        {
            Console.WriteLine("Оберіть навичку, які можна отримати пройшовши курс:");
            Console.WriteLine($"Доступні навички:\n" +
                              "0 - Programming,\n" +
                              "1 - Music,\n" +
                              "2 - Physics,\n" +
                              "3 - HealthCare,\n" +
                              "4 - TimeManagment,\n" +
                              "5 - Communication,\n" +
                              "6 - Illustration,\n" +
                              "7 - Photo\n" +
                              "Введіть номер навика і кількість поінтів через дорівнює (Ось так: 1 = 3)\n");

            var cmdLine = UserInput.NotEmptyString(() => Console.ReadLine());

            Skill newSkill;
            var skillStr = cmdLine?.Split(" = ");

            if (skillStr.Length > 1 && Enum.TryParse(skillStr[0], out SkillKind skillKind) && int.TryParse(skillStr[1], out int points))
            {
                if ((int)skillKind > 7 || (int)skillKind < 0)
                {
                    Console.WriteLine("Такої навички немає\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return null;
                }

                if (points <= 0 | points > 12)
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
    }
}
