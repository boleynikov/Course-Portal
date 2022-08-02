using Data.Repository.Abstract;
using Domain;
using System.Collections.Generic;

namespace EducationPortal.Data.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly List<User> users;

        public UserRepository()
        {
            users = new List<User>(); 
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        public void DeleteByIndex(int index)
        {
            users.Remove(users[index]);
        }

        public User GetByIndex(int index)
        {
            return users[index];
        }

        public void Save()
        {
            //TODO
        }

        public void Update(User user)
        {
            //TODO
        }
    }
}
