// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Context;
    using Data.Repository.Interface;
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
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] OFF");
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var context = _contextFactory.Get();
            var course = context.Courses.FirstOrDefault(u => u.Id == id);
            context.Courses.Remove(course);
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] OFF");
        }

        /// <inheritdoc/>
        public Course GetByID(int id)
        {
            var context = _contextFactory.Get();
            return context.Courses.FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Update(Course editedCourse)
        {
            var context = _contextFactory.Get();
            var course = context.Courses.FirstOrDefault(u => u.Id == editedCourse.Id);
            if (course != null)
            {
                context.Courses.Remove(course);
                context.Courses.Add(editedCourse);
                context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] OFF");
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            var context = _contextFactory.Get();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Courses] OFF");
        }

        /// <inheritdoc/>
        public IEnumerable<Course> GetAll()
        {
            var context = _contextFactory.Get();
            return context.Courses;
        }
    }
}
