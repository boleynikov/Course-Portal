// <copyright file="IController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API.Controllers.Abstract
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
        string Launch();
    }
}
