// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using Data.Repository.Interface;
    using Domain;
    using Services.Interface;
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
        public void Add(User user)
        {
            _repository.Add(user);
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int index)
        {
            _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public IEnumerable<User> GetAll()
        {
            return _repository.GetAll();
        }

        /// <inheritdoc/>
        public User GetByIndex(int index)
        {
            return _repository.GetByIndex(index);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _repository.Save();
        }

        /// <inheritdoc/>
        public void Update(User user)
        {
            _repository.Update(user);
        }
    }
}
