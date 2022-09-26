// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Data.Repository
{
    using Context;
    using Domain.CourseMaterials;
    using Interface;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    /// <summary>
    /// Material Repository.
    /// </summary>
    public class MaterialRepository : IRepository<Material>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRepository"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task Add(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int id)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(u => u.Id == id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Material>> GetAll(int pageNumber)
        {
            //var sql = "EXEC dbo.Material_GetAll";
            //return await _context.Materials.FromSqlRaw<Material>(sql).ToArrayAsync();
            return await _context.Materials.ToArrayAsync();
        }

        /// <inheritdoc/>
        public async Task<Material> GetByID(int id)
        {
            return await _context.Materials.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Update(Material editedMaterial)
        {
            _context.Entry(editedMaterial).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCount()
        {
            return await _context.Materials.CountAsync();
        }
    }
}
