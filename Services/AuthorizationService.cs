// <copyright file="AuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Domain;
    using Interface;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IService<User> _userService;
        private readonly IAuthorizedUserService _authorizedUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="service">User service for decorating.</param>
        /// <param name="authorizedUserService">Service which store signed user</param>
        public AuthorizationService(IService<User> service, IAuthorizedUserService authorizedUserService)
        {
            _userService = service;
            _authorizedUserService = authorizedUserService;
        }

        /// <inheritdoc/>
        public async Task<bool> Login(string email, string password)
        {
            return await TryLogin(email, password);
        }

        /// <inheritdoc/>
        public void Logout()
        {
            _authorizedUserService.Account = null;
        }

        /// <inheritdoc/>
        public async Task<bool> Register(string name, string email, string password)
        {
            var result = await TryRegister(name, email, password);
            return result;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                static string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private async Task<bool> TryLogin(string email, string password)
        {
            var allUsers = await  _userService.GetAll(0);
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if (password == pulledUser.Password)
                {
                    _authorizedUserService.Account = pulledUser;
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> TryRegister(string name, string email, string password)
        {
            var allUsers = await _userService.GetAll(0);
            if (IsValidEmail(email) && allUsers.FirstOrDefault(user => user.Email == email) == null)
            {
                var users = await _userService.GetAll(0);
                var id = users.ToList().Count + 1;
                var newUser = new User(id, name, email, password);
                await _userService.Add(newUser);
                _authorizedUserService.Account = newUser;
                return true;
            }

            return false;
        }
    }
}
