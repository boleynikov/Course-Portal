// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

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
            //var sql = "EXEC dbo.User_GetAll";
            //return await _context.Users.FromSqlRaw<User>(sql).ToArrayAsync();
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

        public async Task<int> GetCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}
