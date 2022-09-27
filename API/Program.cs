// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Services.Validator;

namespace API
{
    using Controllers;
    using Data.Context;
    using Data.Repository;
    using Domain;
    using Domain.CourseMaterials;
    using Services;
    using Services.Helper;
    using Services.Interface;
    using System;
    using System.Text;

    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var context = new ConsoleDbContext();
            IService<Material> materialService = new MaterialService(new MaterialRepository(context));
            IService<Course> courseService = new CourseService(new CourseRepository(context));
            IService<User> userService = new UserService(new UserRepository(context));
            Validator validator = new ();
            IAuthorizedUserService authorizedUserService = new CurrentUserService(userService);
            IAuthorizationService authorizationService = new AuthorizationService(userService, authorizedUserService);
            var page = Command.HomePage;
            while (page != Command.ExitCommand)
            {
                switch (page)
                {
                    case Command.HomePage:
                        page = await new HomeController(courseService, userService, authorizationService, authorizedUserService, validator).Launch();
                        break;
                    case Command.UserPage:
                        page = await new UserController(courseService, materialService, userService, authorizedUserService, validator).Launch();
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
