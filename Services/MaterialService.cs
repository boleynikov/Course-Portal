using Data.Repository.Abstract;
using Domain.CourseMaterials;
using Services.Abstract;

namespace EducationPortal.Services
{
    public class MaterialService : IService<Material>
    {
        private readonly IRepository<Material> repository;

        public MaterialService(IRepository<Material> repository)
        {
            this.repository = repository;
        }
        public void Add(Material material)
        {
            repository.Add(material);
        }

        public void DeleteByIndex(int index)
        {
            repository.DeleteByIndex(index);
        }

        public Material GetByIndex(int index)
        {
           return repository.GetByIndex(index);
        }

        public void Save()
        {
            repository.Save();
        }

        public void Update(Material material)
        {
            repository.Update(material);
        }
    }
}
