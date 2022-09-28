// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using System.Threading.Tasks;
using Domain.CourseMaterials;

namespace Data.Repository
{
    /// <summary>
    /// Material Repository.
    /// </summary>
    public class MaterialFileRepository : IRepository<Material>
    {
        private readonly IDbContext _dbContext;
        private readonly List<Material> _materials;
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialFileRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public MaterialFileRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _materials = Init().ToList();
        }

        private IEnumerable<Material> Init()
        {
            var materials = _dbContext.Get<Material>().Result;
            return materials;
        }

        public async Task Add(Material entity)
        {
            _materials.Add(entity);
            await Save();

        }

        public async Task DeleteByIndex(int id)
        {
            var materialsRes = await _dbContext.Get<Material>();
            var materials = materialsRes.ToList();
            var course = materials.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                materials.Remove(materials[id]);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            await Save();

        }

        public async Task<IEnumerable<Material>> GetAll(int pageNumber)
        {
            if (pageNumber == 0)
            {
                return _materials.ToList();
            }

            return _materials
                .Skip((pageNumber - 1) * 6)
                .Take(6);

        }

        public async Task<Material> GetByID(int id)
        {
            var materialsRes = await _dbContext.Get<Material>();
            return materialsRes.FirstOrDefault(c => c.Id == id);
        }

        public async Task<int> GetCount()
        {
            var materialsRes = await _dbContext.Get<Material>();
            return materialsRes.Count();
        }

        public async Task Save()
        {
            await _dbContext.Update(_materials);

        }

        public async Task Update(Material entity)
        {
            var materialsRes = await _dbContext.Get<Material>();
            var materials = materialsRes.ToList();
            var material = materials.FirstOrDefault(c => c.Id == entity.Id);
            if (material != null)
            {
                int i = materials.IndexOf(material);
                materials[i] = entity;
                await Save();
            }
        }
    }
}
