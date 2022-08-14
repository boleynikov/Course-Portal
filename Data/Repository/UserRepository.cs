// <copyright file="UserRepository.cs" company="PlaceholderCompany">
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
    /// User repository.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly DbContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">DBContextFactory.</param>
        public UserRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        public void Add(User user)
        {
            var context = _contextFactory.Get();
            context.Users.Add(user);
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] OFF");
        }

        /// <inheritdoc/>
        public void DeleteByIndex(int id)
        {
            var context = _contextFactory.Get();
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            context.Users.Remove(user);
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] OFF");
        }

        /// <inheritdoc/>
        public IEnumerable<User> GetAll()
        {
            var context = _contextFactory.Get();
            return context.Users;
        }

        /// <inheritdoc/>
        public User GetByID(int id)
        {
            var context = _contextFactory.Get();
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public void Save()
        {
            var context = _contextFactory.Get();

            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] ON");
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] OFF");
        }

        /// <inheritdoc/>
        public void Update(User editedUser)
        {
            var context = _contextFactory.Get();
            var user = context.Users.FirstOrDefault(u => u.Id == editedUser.Id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.Users.Add(editedUser);
                context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw(@"SET IDENTITY_INSERT [dbo].[Users] OFF");
            }
        }
    }
}
