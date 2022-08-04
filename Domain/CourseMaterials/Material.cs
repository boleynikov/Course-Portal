using Domain.Abstract;

namespace Domain.CourseMaterials
{
    public abstract class Material : BaseEntity
    {
        public string Type { get; set; }
        public string Title { get; set; }

        protected Material(int id, string title, string type) : base(id)
        {
            Title = title;
            Type = type;
        }
    }
}
