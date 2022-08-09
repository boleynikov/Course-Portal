// <copyright file="AuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using System.Linq;
    using Domain;
    using Services.Interface;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IService<User> _userService;
        private User _account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="service">User service for decorating.</param>
        public AuthorizationService(IService<User> service)
        {
            _userService = service;
        }

        /// <inheritdoc/>
        public User GetCurrentAccount() => _account;

        /// <inheritdoc/>
        public bool Login(string email, string password)
        {
            var allUsers = _userService.GetAll();
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if (password == pulledUser.Password)
                {
                    _account = pulledUser;
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void Logout()
        {
            // TODO
        }

        /// <inheritdoc/>
        public void Register(string name, string email, string password)
        {
            var id = _userService.GetAll().Length + 1;
            var newUser = new User(id, name, email, password);
            _userService.Add(newUser);
            _account = newUser;
        }
    }
}
