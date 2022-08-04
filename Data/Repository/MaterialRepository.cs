using Data.Repository.Abstract;
using Domain.CourseMaterials;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class MaterialRepository : IRepository<Material>
    {
        private readonly IDbContext dbContext;
        private readonly List<Material> materials;

        public MaterialRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
            materials = dbContext.Get<Material>();
        }
        public void Add(Material material)
        {
            materials.Add(material);
            Save();
        }

        public void DeleteByIndex(int index)
        {
            materials.Remove(materials[index]);
            Save();
        }

        public Material[] GetAll()
        {
            return materials.ToArray();
        }

        public Material GetByIndex(int index)
        {
            return materials[index];
        }

        public void Save()
        {
            dbContext.Update(materials);
        }

        public void Update(Material editedMaterial)
        {
            var material = materials.FirstOrDefault(u => u.Id == editedMaterial.Id);
            if (material != null)
            {
                int i = materials.IndexOf(material);
                materials[i] = editedMaterial;
                Save();
            }
        }
    }
}
