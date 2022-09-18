// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Services
{
    using Data.Repository.Interface;
    using Domain;
    using Interface;
    using System.Collections.Generic;

    /// <summary>
    /// User Service.
    /// </summary>
    public class UserService : IService<User>
    {
        private readonly IRepository<User> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">Repository instance.</param>
        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task Add(User user)
        {
            await _repository.Add(user);
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int index)
        {
            await _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAll(int pageCount = 0)
        {
            return await _repository.GetAll(pageCount);
        }

        /// <inheritdoc/>
        public async Task<User> GetById(int index)
        {
            return await _repository.GetByID(index);
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _repository.Save();
        }

        /// <inheritdoc/>
        public async Task Update(User user)
        {
            await _repository.Update(user);
        }
    }
}
