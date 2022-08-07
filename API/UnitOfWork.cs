// <copyright file="UnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace API
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Data.Repository.Abstract;
    using Domain;
    using Domain.Abstract;
    using Domain.CourseMaterials;
    using Services;
    using Services.Abstract;

    /// <summary>
    /// Simple implementation of UnitOfWork pattern.
    /// </summary>
    /// <typeparam name="TContext">Type of DBContext.</typeparam>
    public class UnitOfWork<TContext>
        where TContext : IDbContext
    {
        private Dictionary<Type, object> _repositories;
        private Dictionary<Type, object> _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public UnitOfWork(TContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Gets dBContext.
        /// </summary>
        public TContext DbContext { get; }

        /// <summary>
        /// Get Generic Service.
        /// </summary>
        /// <typeparam name="TEntity">Generic type of service.</typeparam>
        /// <returns>TEntity service.</returns>
        public IService<TEntity> GetService<TEntity>()
            where TEntity : BaseEntity
        {
            if (_services == null)
            {
                _services = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_services.ContainsKey(type))
            {
                switch (type.Name)
                {
                    case "Material":
                        _services[type] = new MaterialService(GetRepository<Material>());
                        break;
                    case "User":
                        _services[type] = new UserService(GetRepository<User>());
                        break;
                    case "Course":
                        _services[type] = new CourseService(GetRepository<Course>());
                        break;
                }
            }

            return (IService<TEntity>)_services[type];
        }

        /// <summary>
        /// Get Generic Repository.
        /// </summary>
        /// <typeparam name="TEntity">Generic type of repo.</typeparam>
        /// <returns>TEntity repo.</returns>
        private IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                switch (type.Name)
                {
                    case "Material":
                        _repositories[type] = new MaterialRepository(DbContext);
                        break;
                    case "User":
                        _repositories[type] = new UserRepository(DbContext);
                        break;
                    case "Course":
                        _repositories[type] = new CourseRepository(DbContext);
                        break;
                }
            }

            return (IRepository<TEntity>)_repositories[type];
        }
    }
}
