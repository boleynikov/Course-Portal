namespace Domain.Abstract
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public BaseEntity(int id)
        {
            Id = id;
        }
    }
}
