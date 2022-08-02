using Data.Repository.Abstract;
using Domain.CourseMaterials;
using System.Collections.Generic;

namespace EducationPortal.Data.Repository
{
    public class MaterialRepository : IRepository<Material>
    {
        private readonly List<Material> materials;

        public MaterialRepository()
        {
            materials = new List<Material>();
        }
        public void Add(Material material)
        {
            materials.Add(material);
        }

        public void DeleteByIndex(int index)
        {
            materials.Remove(materials[index]);
        }

        public Material GetByIndex(int index)
        {
            return materials[index];
        }

        public void Save()
        {
            //TODO
        }

        public void Update(Material entity)
        {
            //TODO
        }
    }
}
