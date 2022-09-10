// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Interface;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// User repository.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(u => u.UserMaterials);
        }

        /// <inheritdoc/>
        public User GetByID(int id)
        {
            return _context.Users.Include(u => u.UserMaterials).FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Update(User editedUser)
        {
            _context.Entry(editedUser).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
