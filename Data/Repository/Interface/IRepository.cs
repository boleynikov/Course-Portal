// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository.Interface
{
    /// <summary>
    /// Interface of repository.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adding new T object.
        /// </summary>
        /// <param name="entity">T Object.</param>
        void Add(T entity);

        /// <summary>
        /// Get all T objects.
        /// </summary>
        /// <returns>T array.</returns>
        T[] GetAll();

        /// <summary>
        /// Get T object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>T entity.</returns>
        T GetByIndex(int id);

        /// <summary>
        /// Update T object in repository.
        /// </summary>
        /// <param name="entity">Updated object.</param>
        void Update(T entity);

        /// <summary>
        /// Deleting T object from repo.
        /// </summary>
        /// <param name="id">Object id.</param>
        void DeleteByIndex(int id);

        /// <summary>
        /// Save repository state.
        /// </summary>
        void Save();
    }
}
