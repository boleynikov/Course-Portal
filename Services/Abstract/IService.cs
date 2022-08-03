namespace Services.Abstract
{
    public interface IService<T>
    {
        void Add(T entity);
        T GetByIndex(int index);
        void Update(T entity);
        void DeleteByIndex(int index);
        void Save();
        T[] GetAll();
    }
}
