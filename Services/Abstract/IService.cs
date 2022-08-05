// <copyright file="IService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services.Abstract
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
        void Add(T entity);

        /// <summary>
        /// Get T Entity by id.
        /// </summary>
        /// <param name="index">Entity id.</param>
        /// <returns>Entity.</returns>
        T GetByIndex(int index);

        /// <summary>
        /// Update existing entity in repo.
        /// </summary>
        /// <param name="entity">Updated Entiy.</param>
        void Update(T entity);

        /// <summary>
        /// Delete Entity from repo by id.
        /// </summary>
        /// <param name="index">Entity id.</param>
        void DeleteByIndex(int index);

        /// <summary>
        /// Save current state of repo.
        /// </summary>
        void Save();

        /// <summary>
        /// Get all T entities from repo.
        /// </summary>
        /// <returns>Entities array.</returns>
        T[] GetAll();
    }
}
