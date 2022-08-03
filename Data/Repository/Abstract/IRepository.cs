namespace Data.Repository.Abstract
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T[] GetAll();
        T GetByIndex(int index);
        void Update(T entity);
        void DeleteByIndex(int index);
        void Save();
    }
}
