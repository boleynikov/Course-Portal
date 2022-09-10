// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Interface;
    using Domain.CourseMaterials;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Material Repository.
    /// </summary>
    public class MaterialRepository : IRepository<Material>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Add(Material material)
        {
            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var material = _context.Materials.FirstOrDefault(u => u.Id == id);
            _context.Materials.Remove(material);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<Material> GetAll()
        {
            return _context.Materials;
        }

        /// <inheritdoc/>
        public Material GetByID(int id)
        {
            return _context.Materials.FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Update(Material editedMaterial)
        {
            _context.Entry(editedMaterial).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
