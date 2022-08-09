// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repository.Interface;
    using Domain;

    /// <summary>
    /// Course repository.
    /// </summary>
    public class CourseRepository : IRepository<Course>
    {
        private readonly IDbContext _dbContext;
        private readonly List<Course> _courses;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public CourseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _courses = dbContext.Get<Course>();
        }

        /// <inheritdoc/>
        public void Add(Course course)
        {
            _courses.Add(course);
            Save();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            _courses.Remove(_courses[id]);
            Save();
        }

        /// <inheritdoc/>
        public Course GetByIndex(int id)
        {
            return _courses[id];
        }

        /// <inheritdoc/>
        public void Update(Course editedCourse)
        {
            var course = _courses.FirstOrDefault(u => u.Id == editedCourse.Id);
            if (course != null)
            {
                int i = _courses.IndexOf(course);
                _courses[i] = editedCourse;
                Save();
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            _dbContext.Update(_courses);
        }

        /// <inheritdoc/>
        public Course[] GetAll()
        {
            return _courses.ToArray();
        }
    }
}
