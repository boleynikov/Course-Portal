// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repository.Interface;
    using Domain;

    /// <summary>
    /// User repository.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly IDbContext _dbContext;
        private readonly List<User> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _users = dbContext.Get<User>().ToList();
        }

        /// <inheritdoc/>
        public void Add(User user)
        {
            _users.Add(user);
            Save();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            _users.Remove(_users[id]);
            Save();
        }

        /// <inheritdoc/>
        public IEnumerable<User> GetAll()
        {
            return _users.ToArray();
        }

        /// <inheritdoc/>
        public User GetByIndex(int id)
        {
            return _users.SingleOrDefault(user => user.Id == id);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _dbContext.Update(_users);
        }

        /// <inheritdoc/>
        public void Update(User editedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == editedUser.Id);
            if (user != null)
            {
                int i = _users.IndexOf(user);
                _users[i] = editedUser;
                Save();
            }
        }
    }
}
