// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using System.Threading.Tasks;

namespace Data.Repository
{
    /// <summary>
    /// User repository.
    /// </summary>
    public class UserFileRepository : IRepository<User>
    {
        private readonly IDbContext _dbContext;
        private readonly List<User> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFileRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public UserFileRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _users = Init().ToList();
        }

        private IEnumerable<User> Init()
        {
            var users = _dbContext.Get<User>().Result;
            return users;
        }
        public async Task Add(User entity)
        {
            _users.Add(entity);
            await Save();

        }

        public async Task DeleteByIndex(int id)
        {
            var usersRes = await _dbContext.Get<User>();
            var users = usersRes.ToList();
            var user = users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                users.Remove(users[id]);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            await Save();

        }

        public async Task<IEnumerable<User>> GetAll(int pageNumber)
        {
            if (pageNumber == 0)
            {
                return _users;
            }

            return _users
                .Skip((pageNumber - 1) * 6)
                .Take(6);

        }

        public async Task<User> GetByID(int id)
        {
            var usersRes = await _dbContext.Get<User>();
            return usersRes.FirstOrDefault(c => c.Id == id);
        }

        public async Task<int> GetCount()
        {
            var usersRes = await _dbContext.Get<User>();
            return usersRes.Count();
        }

        public async Task Save()
        { 
            await _dbContext.Update(_users);
        }

        public async Task Update(User entity)
        {
            var usersRes = await _dbContext.Get<User>();
            var users = usersRes.ToList();
            var user = users.FirstOrDefault(c => c.Id == entity.Id);
            if (user != null)
            {
                int i = users.IndexOf(user);
                users[i] = entity;
                await Save();
            }
        }
    }
}
