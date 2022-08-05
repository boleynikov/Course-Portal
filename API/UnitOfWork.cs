﻿using Data.Repository;
using Data.Repository.Abstract;
using Domain;
using Domain.Abstract;
using Domain.CourseMaterials;
using Services;
using Services.Abstract;
using System;
using System.Collections.Generic;

namespace API
{
    public class UnitOfWork<TContext>  where TContext : IDbContext
    {
        private Dictionary<Type, object> _repositories;
        private Dictionary<Type, object> _services;
        public UnitOfWork(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
       
        public TContext DbContext { get; }
       
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
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

        public IService<TEntity> GetService<TEntity>() where TEntity : BaseEntity
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

    }

}
