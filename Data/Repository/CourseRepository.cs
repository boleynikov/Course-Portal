// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Domain.Enum;
using System.Threading.Tasks;

namespace Data.Repository
{
    using Context;
    using Domain;
    using Interface;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Course repository.
    /// </summary>
    public class CourseRepository : IRepository<Course>
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseRepository"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task Add(Course course)
        {
             _context.Courses.Add(course);
             await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByIndex(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(u => u.Id == id);
            if (course != null)
            {
                course.CourseMaterials.Clear();
                course.Status = CourseStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task<Course> GetByID(int id)
        {
            return await _context.Courses.Include(c => c.CourseMaterials).FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task Update(Course editedCourse)
        {
            _context.Entry(editedCourse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Course>> GetAll(int pageNumber = 0)
        {
            if (pageNumber == 0)
            {
                return await _context.Courses.Include(u => u.CourseMaterials).ToArrayAsync();
            }

            return await _context.Courses.Include(u => u.CourseMaterials)
                                         .Skip((pageNumber - 1) * 6)
                                         .Take(6)
                                         .ToArrayAsync();
        }

        public async Task<int> GetCount()
        {
            return await _context.Courses.CountAsync();
        }
    }
}
