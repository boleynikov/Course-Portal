// <copyright file="IDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository.Interface
{
    using System.Collections.Generic;
    using Domain.Abstract;

    /// <summary>
    /// Interface of DBContext.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Getting T list objects.
        /// </summary>
        /// <typeparam name="T">Type of objects in list.</typeparam>
        /// <returns>List of T objects.</returns>
        List<T> Get<T>()
            where T : BaseEntity;

        /// <summary>
        /// Saving T list objects.
        /// </summary>
        /// <typeparam name="T">Type of objects.</typeparam>
        /// <param name="listEntities">Type of objects in list.</param>
        /// <returns>Result.</returns>
        bool Update<T>(List<T> listEntities)
            where T : BaseEntity;
    }
}
