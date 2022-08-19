// <copyright file="CourseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
        public void Add(Course course)
        {
            _repository.Add(course);
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int index)
        {
            _repository.DeleteByIndex(index);
        }

        /// <inheritdoc/>
        public IEnumerable<Course> GetAll()
        {
            return _repository.GetAll();
        }

        /// <inheritdoc/>
        public Course GetById(int index)
        {
            return _repository.GetByID(index);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _repository.Save();
        }

        /// <inheritdoc/>
        public void Update(Course course)
        {
            _repository.Update(course);
        }
    }
}
