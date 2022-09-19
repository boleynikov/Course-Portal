// <copyright file="MaterialService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Services
{
    using Data.Repository.Interface;
    using Domain.CourseMaterials;
    using Interface;
    using System.Collections.Generic;

    /// <summary>
    /// Material Service.
    /// </summary>
    public class MaterialService : IService<Material>
    {
        private readonly IRepository<Material> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialService"/> class.
        /// </summary>
        /// <param name="repository">Repository instance.</param>
        public MaterialService(IRepository<Material> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task Add(Material material)
        {
            await _repository.Add(material);
        }
        public async Task<int> GetCount()
        {
            return await _repository.GetCount();
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int index)
        {
            await _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Material>> GetAll(int pageNumber = 0)
        {
            return await _repository.GetAll(pageNumber);
        }

        /// <inheritdoc/>
        public async Task<Material> GetById(int index)
        {
            return await _repository.GetByID(index);
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _repository.Save();
        }

        /// <inheritdoc/>
        public async Task Update(Material material)
        {
            await _repository.Update(material);
        }
    }
}
