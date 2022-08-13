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
    using Services.Helper;
    using Services.Interface;

    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var context = new FileDbContext();

            IService<Material> materialService = new MaterialService(new MaterialRepository(context));
            IService<Course> courseService = new CourseService(new CourseRepository(context));
            IService<User> userService = new UserService(new UserRepository(context));
            Validator validator = new Validator();
            IAuthorizationService authorizationService = new AuthorizationService(userService, validator);

            string page = Command.HomePage;
            while (page != Command.ExitCommand)
            {
                switch (page)
                {
                    case Command.HomePage:
                        page = new HomeController(courseService, userService, authorizationService, validator).Launch();
                        break;
                    case Command.UserPage:
                        page = new UserController(courseService, materialService, userService, authorizationService, validator).Launch();
                        break;
                    case Command.BackCommand:
                        page = Command.HomePage;
                        break;
                    default:
                        Console.WriteLine("Невідома сторінка\nНатисніть Enter");
                        Console.ReadLine();
                        page = Command.HomePage;
                        break;
                }
            }
        }
    }
}
