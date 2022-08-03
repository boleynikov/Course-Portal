using Data.Repository.Abstract;
using Domain;
using Services.Abstract;

namespace Services
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> repository;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
        }
        public void Add(User user)
        {
            repository.Add(user);
        }

        public void DeleteByIndex(int index)
        {
            repository.DeleteByIndex(index);
        }

        public User[] GetAll()
        {
            return repository.GetAll();
        }

        public User GetByIndex(int index)
        {
            return repository.GetByIndex(index);
        }

        public void Save()
        {
            repository.Save();
        }

        public void Update(User user)
        {
            repository.Update(user);
        }
    }
}
