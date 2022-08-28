// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Interface;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Course repository.
    /// </summary>
    public class CourseRepository : IRepository<Course>
    {
        private readonly DbContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public CourseRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        public void Add(Course course)
        {
            var context = _contextFactory.Get();
            context.Courses.Add(course);
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var context = _contextFactory.Get();
            var course = context.Courses.FirstOrDefault(u => u.Id == id);
            context.Courses.Remove(course);
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public Course GetByID(int id)
        {
            var context = _contextFactory.Get();
            return context.Courses.Include(c => c.CourseMaterials).FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Update(Course editedCourse)
        {
            var context = _contextFactory.Get();
            context.Entry(editedCourse).State = EntityState.Modified;
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Save()
        {
            var context = _contextFactory.Get();
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<Course> GetAll()
        {
            var context = _contextFactory.Get();
            return context.Courses.Include(c => c.CourseMaterials);
        }
    }
}
