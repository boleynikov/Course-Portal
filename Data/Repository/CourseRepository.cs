// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Domain.Enum;

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
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Add(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var course = _context.Courses.FirstOrDefault(u => u.Id == id);
            if (course != null)
            {
                course.CourseMaterials.Clear();
                course.Status = CourseStatus.Deleted;
                _context.SaveChanges();
            }
        }

        /// <inheritdoc/>
        public Course GetByID(int id)
        {
            return _context.Courses.Include(c => c.CourseMaterials).FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Update(Course editedCourse)
        {
            _context.Entry(editedCourse).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<Course> GetAll()
        {
            return _context.Courses.Include(c => c.CourseMaterials);
        }
    }
}
