// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Data.Repository
{
    using Context;
    using Domain;
    using Interface;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    /// <summary>
    /// User repository.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAll(int pageNumber)
        {
            return await _context.Users.Include(u => u.UserMaterials).ToArrayAsync();
        }

        /// <inheritdoc/>
        public async Task<User> GetByID(int id)
        {
            return await _context.Users.Include(u => u.UserMaterials).FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Update(User editedUser)
        {
            _context.Entry(editedUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> GetCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}
