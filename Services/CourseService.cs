// <copyright file="CourseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Services
{
    using Data.Repository.Interface;
    using Domain;
    using Interface;
    using System.Collections.Generic;

    /// <summary>
    /// CourseService.
    /// </summary>
    public class CourseService : IService<Course>
    {
        private readonly IRepository<Course> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class.
        /// </summary>
        /// <param name="repository">Repository instance.</param>
        public CourseService(IRepository<Course> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task Add(Course course)
        {
            await _repository.Add(course);
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int index)
        {
            await _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Course>> GetAll(int pageNumber = 0)
        {
            return await _repository.GetAll(pageNumber);
        }

        /// <inheritdoc/>
        public async Task<Course> GetById(int index)
        {
            return await _repository.GetByID(index);
        }

        public async Task<int> GetCount()
        {
            return await _repository.GetCount();
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _repository.Save();
        }

        /// <inheritdoc/>
        public async Task Update(Course course)
        {
            await _repository.Update(course);
        }
    }
}
