// <copyright file="AuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
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
            if (IsValidEmail(email))
            {
                var id = _userService.GetAll().ToList().Count + 1;
                var newUser = new User(id, name, email, password);
                _userService.Add(newUser);
                _account = newUser;
            }
            else
            {
                Console.WriteLine("E-mail у неправильному форматі\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
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
    }
}
