using API.Controllers;
using API.Controllers.Abstract;
using Data.Repository;
using Data.Repository.Abstract;
using Domain;
using Domain.CourseMaterials;
using Services;
using Services.Abstract;
using System;
using System.Text;

namespace API
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            var unitOfWork = new UnitOfWork<FileDbContext>(new FileDbContext());

            var materialRepository = unitOfWork.GetRepository<Material>();
            var courseRepository = unitOfWork.GetRepository<Course>();
            var userRepository = unitOfWork.GetRepository<User>();

            var materialService = unitOfWork.GetService<Material>();
            var courseService =  unitOfWork.GetService<Course>();
            var userService =  unitOfWork.GetService<User>();


            IAuthenticationService authenticationService = new AuthenticationService(userService);

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
