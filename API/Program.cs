// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API
{
    using System;
    using System.Text;
    using API.Controllers;
    using API.Controllers.Abstract;
    using API.Interface;
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

            IUnitOfWork<FileDbContext> unitOfWork = new UnitOfWork<FileDbContext>(new FileDbContext());

            var materialService = unitOfWork.GetService<Material>();
            var courseService = unitOfWork.GetService<Course>();
            var userService = unitOfWork.GetService<User>();

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
                        int userId = authorizationService.GetCurrentAccount().Id;
                        page = new UserController(courseService, materialService, userService, userId).Launch();
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
