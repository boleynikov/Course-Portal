using Domain;
using Services.Abstract;
using System;

namespace EducationPortal.Services
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly IService<User> service;

        public AuthenticationService(IService<User> service)
        {
            this.service = service;
        }
        public void Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void Register(string name, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
