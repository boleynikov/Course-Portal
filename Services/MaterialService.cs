// <copyright file="MaterialService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using Data.Repository.Interface;
    using Domain.CourseMaterials;
    using Services.Interface;
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
        public void Add(Material material)
        {
            _repository.Add(material);
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int index)
        {
            _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public IEnumerable<Material> GetAll()
        {
            return _repository.GetAll();
        }

        /// <inheritdoc/>
        public Material GetByIndex(int index)
        {
            return _repository.GetByIndex(index);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _repository.Save();
        }

        /// <inheritdoc/>
        public void Update(Material material)
        {
            _repository.Update(material);
        }
    }
}
