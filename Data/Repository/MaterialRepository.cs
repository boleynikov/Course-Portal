// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repository.Interface;
    using Domain.CourseMaterials;

    /// <summary>
    /// Material Repository.
    /// </summary>
    public class MaterialRepository : IRepository<Material>
    {
        private readonly IDbContext _dbContext;
        private readonly List<Material> _materials;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public MaterialRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _materials = dbContext.Get<Material>().ToList();
        }

        /// <inheritdoc/>
        public void Add(Material material)
        {
            _materials.Add(material);
            Save();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            _materials.Remove(_materials[id]);
            Save();
        }

        /// <inheritdoc/>
        public IEnumerable<Material> GetAll()
        {
            return _materials.ToArray();
        }

        /// <inheritdoc/>
        public Material GetByID(int id)
        {
            var material = _materials.FirstOrDefault(c => c.Id == id);
            return material ?? throw new ArgumentOutOfRangeException(nameof(id));
        }

        /// <inheritdoc/>
        public void Save()
        {
            _dbContext.Update(_materials);
        }

        /// <inheritdoc/>
        public void Update(Material editedMaterial)
        {
            var material = _materials.FirstOrDefault(u => u.Id == editedMaterial.Id);
            if (material != null)
            {
                int i = _materials.IndexOf(material);
                _materials[i] = editedMaterial;
                Save();
            }
        }
    }
}
