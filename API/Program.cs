// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API
{
    using System;
    using System.Text;
    using API.Controllers;
    using API.Controllers.Abstract;
    using Data.Repository;
    using Data.Repository.Abstract;
    using Domain;
    using Domain.CourseMaterials;
    using Services;
    using Services.Abstract;

    /// <summary>
    /// Start Program class.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var unitOfWork = new UnitOfWork<FileDbContext>(new FileDbContext());

            var materialService = unitOfWork.GetService<Material>();
            var courseService = unitOfWork.GetService<Course>();
            var userService = unitOfWork.GetService<User>();

            IAuthorizationService authenticationService = new AuthorizationService(userService);

            IController home = new HomeController(courseService, userService, authenticationService);
            IController user;

            string page = "home";
            while (page != "exit")
            {
                switch (page)
                {
                    case "home":
                        page = home.Launch();
                        break;
                    case "user":
                        int userId = authenticationService.GetCurrentAccount().Id;
                        user = new UserController(courseService, materialService, userService, userId);
                        page = user.Launch();
                        break;
                    default:
                        Console.WriteLine("Невідома сторінка\nНатисніть Enter");
                        page = "home";
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
