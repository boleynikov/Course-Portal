// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using ConsoleAPI.Controllers;
using Data.Context;
using Data.Repository;
using Data.Repository.Interface;
using Domain;
using Domain.CourseMaterials;
using Services;
using Services.Helper;
using Services.Interface;
using Services.Validator;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            //Using SQL Server
            //var context = new ConsoleDbContext();
            //IService<Material> materialService = new MaterialService(new MaterialRepository(context));
            //IService<Course> courseService = new CourseService(new CourseRepository(context));
            //IService<User> userService = new UserService(new UserRepository(context));
            //Validator validator = new Validator();
            //IAuthorizedUserService authorizedUserService = new CurrentUserService(userService);
            //IAuthorizationService authorizationService = new AuthorizationService(userService, authorizedUserService);

            //In case of using File System for storage - uncomment
            //
            IDbContext context = new FileDbContext();
            IService<Material> materialService = new MaterialService(new MaterialFileRepository(context));
            IService<Course> courseService = new CourseService(new CourseFileRepository(context));
            IService<User> userService = new UserService(new UserFileRepository(context));
            Validator validator = new Validator();
            IAuthorizedUserService authorizedUserService = new CurrentUserService(userService);
            IAuthorizationService authorizationService = new AuthorizationService(userService, authorizedUserService);

            var page = Command.HomePage;
            while (page != Command.ExitCommand)
            {
                switch (page)
                {
                    case Command.HomePage:
                        page = await new HomeController(courseService, userService, materialService, authorizationService, authorizedUserService, validator).Launch();
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
