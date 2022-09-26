// <copyright file="IAuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Services.Interface
{

    /// <summary>
    /// Interface of Authorization Service.
    /// </summary>
    public interface IAuthorizationService
    {

        /// <summary>
        /// Login to system.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Result of authorisation.</returns>
        Task<bool> Login(string email, string password);

        /// <summary>
        /// Logout from user account.
        /// </summary>
        void Logout();

        /// <summary>
        /// Register in system/Create new user account.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        Task<bool> Register(string name, string email, string password);
    }
}
