﻿using Data.Repository.Abstract;
using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly IDbContext dbContext;
        private readonly List<User> users;

        public UserRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
            users = dbContext.Get<User>();
        }

        public void Add(User user)
        {
            users.Add(user);
            Save();
        }

        public void DeleteByIndex(int index)
        {
            users.Remove(users[index]);
            Save();
        }

        public User[] GetAll()
        {
            return users.ToArray();
        }

        public User GetByIndex(int index)
        {
            return users.SingleOrDefault(user => user.Id == index);
        }

        public void Save()
        {
            dbContext.Update(users);
        }

        public void Update(User editedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == editedUser.Id);
            if(user != null)
            {
                int i = users.IndexOf(user);
                users[i] = editedUser;
                Save();
            }
        }
    }
}
