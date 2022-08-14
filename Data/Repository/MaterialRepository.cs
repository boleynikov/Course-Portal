// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Context;
    using Data.Repository.Interface;
    using Domain.CourseMaterials;

    /// <summary>
    /// Material Repository.
    /// </summary>
    public class MaterialRepository : IRepository<Material>
    {
        private readonly DbContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public MaterialRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        public void Add(Material material)
        {
            var context = _contextFactory.Get();
            context.Materials.Add(material);
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var context = _contextFactory.Get();
            var material = context.Materials.FirstOrDefault(u => u.Id == id);
            context.Materials.Remove(material);
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<Material> GetAll()
        {
            var context = _contextFactory.Get();
            return context.Materials;
        }

        /// <inheritdoc/>
        public Material GetByID(int id)
        {
            var context = _contextFactory.Get();
            return context.Materials.FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Save()
        {
            var context = _contextFactory.Get();
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Update(Material editedMaterial)
        {
            var context = _contextFactory.Get();
            var material = context.Materials.FirstOrDefault(u => u.Id == editedMaterial.Id);
            if (material != null)
            {
                context.Materials.Remove(material);
                context.Materials.Add(editedMaterial);
                context.SaveChanges();
            }
        }
    }
}
