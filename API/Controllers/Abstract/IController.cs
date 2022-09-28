// <copyright file="IController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace ConsoleAPI.Controllers.Abstract
{
    /// <summary>
    /// Interface of Controller.
    /// </summary>
    internal interface IController
    {
        /// <summary>
        /// Launch controller.
        /// </summary>
        /// <returns>String of page to redirect.</returns>
        Task<string> Launch();
    }
}
