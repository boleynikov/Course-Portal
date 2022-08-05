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

            var fileContext = new FileDbContext();

            IRepository<Material> materialRepository = new MaterialRepository(fileContext);
            IService<Material> materialService = new MaterialService(materialRepository);

            IRepository<Course> courseRepository = new CourseRepository(fileContext);
            IService<Course> courseService = new CourseService(courseRepository);

            IRepository<User> userRepository = new UserRepository(fileContext);
            IService<User> userService = new UserService(userRepository);
            IAuthenticationService authenticationService = new AuthenticationService(userService);

            IController home = new HomeController(courseService, userService, authenticationService);
            IController user;

            string page = "home";
            while(page != "exit")
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
