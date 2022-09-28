// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using ConsoleAPI.Controllers.Abstract;
using ConsoleAPI.View;
using Domain;
using Domain.CourseMaterials;
using Services;
using Services.Helper;
using Services.Interface;
using Services.Validator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleAPI.Controllers
{
    /// <summary>
    /// User Controller.
    /// </summary>
    public class UserController : IController
    {
        private readonly IService<Course> _courseService;
        private readonly IService<Material> _materialService;
        private readonly IService<User> _userService;
        private readonly Validator _validateService;
        private readonly IAuthorizedUserService _authorizedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="courseService">Course service instance</param>
        /// <param name="materialService">Material service instance</param>
        /// <param name="userService">User service instance</param>
        /// <param name="authorizedUser">Current authorized user service</param>
        /// <param name="validateService">Input validation service</param>
        public UserController(
            IService<Course> courseService,
            IService<Material> materialService,
            IService<User> userService,
            IAuthorizedUserService authorizedUser,
            Validator validateService)
        {
            _courseService = courseService;
            _materialService = materialService;
            _userService = userService;
            _authorizedUser = authorizedUser;
            _validateService = validateService;
        }

        /// <inheritdoc/>
        public async Task<string> Launch()
        {
            var currentUser = _authorizedUser.Account;
            var page = Command.UserPage;

            while (page == Command.UserPage)
            {
                Console.Clear();
                var userCourses = currentUser.UserCourses.Select(c => c.Key).ToList();
                var courses = _courseService.GetAll(0).Result.Where(c => userCourses.Contains(c.Id));
                UserPageView.Show(currentUser, courses.ToList());

                var cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case Command.CreateCourseCommand:
                        Console.Write("Введіть назву курсу: ");
                        var name = UserInput.NotEmptyString(() => Console.ReadLine());
                        Console.Write("Введіть опис курсу: ");
                        var description = UserInput.NotEmptyString(() => Console.ReadLine());
                        var course = await _authorizedUser.CreateCourse(name, description, _authorizedUser.Account.Name, _courseService, _materialService);
                        await _authorizedUser.AddCourseToUser(course.Id);
                        await _userService.Save();
                        await _courseService.Add(course);
                        Console.Write("Курс успішно додано. Натисніть Enter");
                        Console.ReadLine();
                        break;
                    case Command.OpenCourseCommand:
                        Console.Write("Введіть номер курсу: ");
                        if (_validateService.Course.Validate(courses.ToList(), Console.ReadLine(), out course))
                        {
                            page = await new CourseController(_userService, _courseService, _materialService, _authorizedUser, new OpenedCourseService(course, _validateService), Command.UserPage).Launch();
                        }

                        break;
                    case Command.DeleteCourseCommand:
                        Console.Write("Введіть номер курсу: ");
                        if (_validateService.Course.Validate(courses.ToList(), Console.ReadLine(), out course))
                        {
                            _authorizedUser.RemoveCourse(course.Id);
                            await _userService.Update(currentUser);
                            await _courseService.DeleteByIndex(course.Id);
                        }

                        break;
                    case Command.BackCommand:
                        page = Command.BackCommand;
                        break;
                }
            }

            return page;
        }
    }
}