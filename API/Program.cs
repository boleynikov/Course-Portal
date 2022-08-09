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
        private const string _exit = "6";

        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var unitOfWork = new UnitOfWork<FileDbContext>(new FileDbContext());

            var materialService = unitOfWork.GetService<Material>();
            var courseService = unitOfWork.GetService<Course>();
            var userService = unitOfWork.GetService<User>();

            IAuthorizationService authorizationService = new AuthorizationService(userService);

            IController home = new HomeController(courseService, userService, authorizationService);
            IController user;

            string page = _homePage;
            while (page != _exit)
            {
                switch (page)
                {
                    case _homePage:
                        page = home.Launch();
                        break;
                    case _userPage:
                        int userId = authorizationService.GetCurrentAccount().Id;
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
