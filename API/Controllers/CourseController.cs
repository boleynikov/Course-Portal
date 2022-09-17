// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Domain.CourseMaterials;
using Domain.Enum;

namespace API.Controllers
{
    using System;
    using Abstract;
    using Domain;
    using Services.Helper;
    using Services.Interface;
    using View;

    /// <summary>
    /// Course Controller.
    /// </summary>
    public class CourseController : IController
    {
        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;
        private readonly IService<Material> _materialService;
        private readonly IAuthorizedUserService _authorizedUser;
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
            IService<Material> materialService,
            IAuthorizedUserService authorizedUser,
            IOpenedCourseService openedCourse,
            string redirectPage = "home")
        {
            _userService = userService;
            _courseService = courseService;
            _materialService = materialService;
            _openedCourse = openedCourse;
            _authorizedUser = authorizedUser;
            _redirectPage = redirectPage;
        }

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
            string page = Command.CoursePage;
            while (page == Command.CoursePage)
            {
                Console.Clear();
                var currentUser = _authorizedUser.Account;
                var currentCourse = _openedCourse.Get();
                CoursePageView.Show(currentUser, currentCourse);
                string cmdLine = Console.ReadLine();
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

                            _authorizedUser.AddCourseToUser(currentCourse);
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
                        _openedCourse.EditCourseName();
                        break;
                    case Command.EditCourseDescription:
                        _openedCourse.EditCourseDescription();
                        break;
                    case Command.DeleteCourseMaterial:
                        //try
                        //{
                        //    int matId = _openedCourse.DeleteCourseMaterial();
                        //    if (_materialService.GetById(matId).User.Id == _authorizedUser.Account.Id)
                        //    {
                        //        _materialService.DeleteByIndex(matId);
                        //    }
                        //}
                        //catch
                        //{
                        //    Console.WriteLine("Такого ідентифікатору немає");
                        //}

                        break;
                    case Command.AddCourseMaterials:
                        _openedCourse.AddCourseMaterial(_authorizedUser.Account.UserMaterials.ToList());
                        break;
                    case Command.AddNewOrEditSkill:
                        _openedCourse.AddOrEditSkill();
                        break;
                    case Command.DeleteSkill:
                        _openedCourse.DeleteSkill();
                        break;
                }
            }
        }
    }
}
