﻿// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Data;

namespace API
{
    using System;
    using System.Text;
    using Controllers;
    using Data.Context;
    using Data.Repository;
    using Domain;
    using Domain.CourseMaterials;
    using Services;
    using Services.Helper;
    using Services.Interface;
    using Services.Validators;

    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            //IService<Material> materialService = new MaterialService(new MaterialRepository(contextFactory));
            //IService<Course> courseService = new CourseService(new CourseRepository(contextFactory));
            //IService<User> userService = new UserService(new UserRepository(contextFactory));
            //Validator validator = new ();
            //IAuthorizedUserService authorizedUserService = new AuthorizedUserService(userService, validator);
            //IAuthorizationService authorizationService = new AuthorizationService(userService, authorizedUserService);
            //string page = Command.HomePage;
            //while (page != Command.ExitCommand)
            //{
            //    switch (page)
            //    {
            //        case Command.HomePage:
            //            page = new HomeController(courseService, userService, materialService, authorizationService, authorizedUserService, validator).Launch();
            //            break;
            //        case Command.UserPage:
            //            page = new UserController(courseService, materialService, userService, authorizedUserService, validator).Launch();
            //            break;
            //        case Command.BackCommand:
            //            page = Command.HomePage;
            //            break;
            //        default:
            //            Console.WriteLine("Невідома сторінка\nНатисніть Enter");
            //            Console.ReadLine();
            //            page = Command.HomePage;
            //            break;
            //    }
            //}

            //contextFactory.Get().Dispose();
        }
    }
}
