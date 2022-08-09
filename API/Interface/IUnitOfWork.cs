using Data.Repository.Interface;
using Domain.Abstract;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Interface
{
    /// <summary>
    /// Interface of unitOfWork
    /// </summary>
    public interface IUnitOfWork<TContext>
        where TContext : IDbContext
    {
        /// <summary>
        /// Get Db Context
        /// </summary>
        public TContext DbContext { get; }

        /// <summary>
        /// Get Service of TEntity
        /// </summary>
        /// <typeparam name="TEntity">Type of service</typeparam>
        /// <returns>Service of type TEntity</returns>
        IService<TEntity> GetService<TEntity>()
            where TEntity : BaseEntity;

        /// <summary>
        /// Get Repository of TEntity
        /// </summary>
        /// <typeparam name="TEntity">Type of repository</typeparam>
        /// <returns>Repository of type TEntity</returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity;
    }
}
