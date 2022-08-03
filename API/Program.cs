using API.Controllers;
using Data.Repository;
using Data.Repository.Abstract;
using Domain;
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

            IRepository<Course> courseRepository = new CourseRepository(new FileDbContext());
            IService<Course> courseService = new CourseService(courseRepository);

            IRepository<User> userRepository = new UserRepository(new FileDbContext());
            IService<User> userService = new UserService(userRepository);
            IAuthenticationService authenticationService = new AuthenticationService(userService);

            HomeController home = new HomeController(courseService, authenticationService);

            home.Launch();
        }
    }
}
