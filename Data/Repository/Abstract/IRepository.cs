namespace Data.Repository.Abstract
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T[] GetAll();
        T GetByIndex(int id);
        void Update(T entity);
        void DeleteByIndex(int id);
        void Save();
    }
}
