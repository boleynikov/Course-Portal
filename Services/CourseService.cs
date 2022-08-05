// <copyright file="CourseService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using Data.Repository.Abstract;
    using Domain;
    using Services.Abstract;

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
        public Course[] GetAll()
        {
            return _repository.GetAll();
        }

        /// <inheritdoc/>
        public Course GetByIndex(int index)
        {
            return _repository.GetByIndex(index);
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
