using Domain;
using Services.Abstract;
using System.Linq;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IService<User> userService;
        private User account;
        public AuthenticationService(IService<User> service)
        {
            this.userService = service;
        }

        public User GetCurrentAccount() => account;
        public bool Login(string email, string password)
        {
            var allUsers = userService.GetAll();
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if(password == pulledUser.Password)
                {
                    account = pulledUser;
                    return true;
                }
                else
                {                    
                    return false;
                }
            }
            else
            {                
                return false;
            }
        }

        public void Logout()
        {
            //TODO
        }

        public void Register(string name, string email, string password)
        {
            var id = userService.GetAll().Length + 1; 
            var newUser = new User(id, name, email, password);
            userService.Add(newUser);
            account = newUser;
        }
    }
}
