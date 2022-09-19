// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    /// <summary>
    /// Interface of repository.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get count of objects
        /// </summary>
        /// <returns></returns>
        Task<int> GetCount();
        /// <summary>
        /// Adding new T object.
        /// </summary>
        /// <param name="entity">T Object.</param>
        Task Add(T entity);

        /// <summary>
        /// Get all T objects.
        /// </summary>
        /// <returns>T array.</returns>
        Task<IEnumerable<T>> GetAll(int pageNumber);

        /// <summary>
        /// Get T object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>T entity.</returns>
        Task<T> GetByID(int id);

        /// <summary>
        /// Update T object in repository.
        /// </summary>
        /// <param name="entity">Updated object.</param>
        Task Update(T entity);

        /// <summary>
        /// Deleting T object from repo.
        /// </summary>
        /// <param name="id">Object id.</param>
        Task DeleteByIndex(int id);

        /// <summary>
        /// Save repository state.
        /// </summary>
        Task Save();
    }
}
