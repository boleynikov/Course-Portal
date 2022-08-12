// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API
{
    using System;
    using System.Text;
    using API.Controllers;
    using Data.Repository;
    using Domain;
    using Domain.CourseMaterials;
    using Services;
    using Services.Interface;

    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private const string _homePage = "home";
        private const string _userPage = "1";
        private const string _exit = "7";

        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var context = new FileDbContext();

            IService<Material> materialService = new MaterialService(new MaterialRepository(context));
            IService<Course> courseService = new CourseService(new CourseRepository(context));
            IService<User> userService = new UserService(new UserRepository(context));

            IAuthorizationService authorizationService = new AuthorizationService(userService);

            string page = _homePage;
            while (page != _exit)
            {
                switch (page)
                {
                    case _homePage:
                        page = new HomeController(courseService, userService, authorizationService).Launch();
                        break;
                    case _userPage:
                        page = new UserController(courseService, materialService, userService, authorizationService).Launch();
                        break;
                    default:
                        Console.WriteLine("Невідома сторінка\nНатисніть Enter");
                        Console.ReadLine();
                        page = "home";
                        break;
                }
            }
        }
    }
}
