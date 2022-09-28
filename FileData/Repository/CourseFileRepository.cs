// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Data.Repository.Interface;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    /// <summary>
    /// Course repository.
    /// </summary>
    public class CourseFileRepository : IRepository<Course>
    {
        private readonly IDbContext _dbContext;
        private readonly List<Course> _courses;
        /// <summary>
        /// Initializes a new instance of the <see cref="CourseFileRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DBContext.</param>
        public CourseFileRepository(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _courses = Init().ToList();
        }

        private IEnumerable<Course> Init()
        {
            var courses = _dbContext.Get<Course>().Result;
            return courses;
        }
        public async Task Add(Course entity)
        {
            _courses.Add(entity);
            await Save();

        }

        public async Task DeleteByIndex(int id)
        {
            var coursesRes = await _dbContext.Get<Course>();
            var courses = coursesRes.ToList();
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                courses.Remove(courses[id]);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            await Save();

        }

        public async Task<IEnumerable<Course>> GetAll(int pageNumber)
        {
            if (pageNumber == 0)
            {
                return _courses.ToList();
            }

            return _courses
                .Skip((pageNumber - 1) * 6)
                .Take(6);

        }

        public async Task<Course> GetByID(int id)
        {
            var coursesRes = await _dbContext.Get<Course>();
            return coursesRes.FirstOrDefault(c => c.Id == id);
        }

        public async Task<int> GetCount()
        {
            var coursesRes = await _dbContext.Get<Course>();
            return coursesRes.Count();
        }

        public async Task Save()
        {
            await _dbContext.Update(_courses);
        }

        public async Task Update(Course entity)
        {
            var coursesRes = await _dbContext.Get<Course>();
            var courses = coursesRes.ToList();
            var course = courses.FirstOrDefault(c => c.Id == entity.Id);
            if (course != null)
            {
                var i = courses.IndexOf(course);
                courses[i] = entity;
                await Save();
            }
        }
    }
}
