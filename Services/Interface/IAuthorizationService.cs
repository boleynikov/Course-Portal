// <copyright file="IAuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services.Interface
{
    using Domain;

    /// <summary>
    /// Interface of Authorization Service.
    /// </summary>
    public interface IAuthorizationService : IAuthorizedUserService
    {
        /// <summary>
        /// Get current authorized account.
        /// </summary>
        /// <returns>User instance.</returns>
        User Get();

        /// <summary>
        /// Login to system.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Result of authorisation.</returns>
        void Login(string email, string password);

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
        void Register(string name, string email, string password);
    }
}
