// <copyright file="IService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    /// <summary>
    /// Generic Interface for all services.
    /// </summary>
    /// <typeparam name="T">Type of service.</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Add new entity.
        /// </summary>
        /// <param name="entity">Entity, which derives DBseEntity.</param>
        Task Add(T entity);

        /// <summary>
        /// Get T Entity by id.
        /// </summary>
        /// <param name="index">Entity id.</param>
        /// <returns>Entity.</returns>
        Task<T> GetById(int index);

        /// <summary>
        /// Update existing entity in repo.
        /// </summary>
        /// <param name="entity">Updated Entiy.</param>
        Task Update(T entity);

        /// <summary>
        /// Delete Entity from repo by id.
        /// </summary>
        /// <param name="index">Entity id.</param>
        Task DeleteByIndex(int index);

        /// <summary>
        /// Save current state of repo.
        /// </summary>
        Task Save();

        /// <summary>
        /// Get all T entities from repo.
        /// </summary>
        /// <returns>Entities array.</returns>
        Task<IEnumerable<T>> GetAll(int pageNumber);
        /// <summary>
        /// Get objects count
        /// </summary>
        /// <returns></returns>
        Task<int> GetCount();
    }
}
