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
        public async Task Login(string email, string password)
        {
            await TryLogin(email, password);
            //var loginResult = await TryLogin(email, password);
            //if (loginResult)
            //{
            //    Console.WriteLine($"З поверненням {_authorizedUserService.Account.Name}\n" +
            //                       "Натисніть Enter");
            //    Console.ReadLine();
            //}
            //else
            //{
            //    Console.WriteLine("Невірний email чи пароль");
            //    Console.ReadLine();
            //}
        }

        /// <inheritdoc/>
        public void Logout()
        {
            _authorizedUserService.Account = null;
        }

        /// <inheritdoc/>
        public async Task Register(string name, string email, string password)
        {
            await TryRegister(name, email, password);
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
            var allUsers = await  _userService.GetAll();
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

        private async Task TryRegister(string name, string email, string password)
        {
            if (IsValidEmail(email))
            {
                var users = await _userService.GetAll();
                var id = users.ToList().Count + 1;
                var newUser = new User(id, name, email, password);
                await _userService.Add(newUser);
                _authorizedUserService.Account = newUser;
            }
            else
            {
                Console.WriteLine("E-mail у неправильному форматі\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
            }
        }
    }
}
