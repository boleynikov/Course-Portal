using Domain;
using Services.Abstract;
using System;
using System.Linq;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IService<User> userService;

        public AuthenticationService(IService<User> service)
        {
            this.userService = service;
        }
        public bool Login(string email, string password)
        {
            var allUsers = userService.GetAll();
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if(password == pulledUser.Password)
                {
                    Console.WriteLine($"З поверненням, {pulledUser.Name}");
                    return true;
                }
                else
                {
                    Console.WriteLine("Невірний пароль");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Облікового запису з такою поштою немає");
                return false;
            }
        }

        public void Logout()
        {
            //TODO
        }

        public void Register(string name, string email, string password)
        {
            var user = new User(name, email, password);
            userService.Add(user);
        }
    }
}
