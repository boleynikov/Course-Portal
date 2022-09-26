using Domain.CourseMaterials;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Validator
{
    public class MaterialsValidator : IValidateService<Material>
    {
        public bool Validate(List<Material> userOrCourseMaterials, string strEntityId, out Material entity)
        {

            if (int.TryParse(strEntityId, out int materialId))
            {
                try
                {
                    entity = userOrCourseMaterials.FirstOrDefault(c => c.Id == materialId)
                        ?? throw new ArgumentOutOfRangeException(nameof(materialId));
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    entity = null;
                    Console.WriteLine($"Немає матеріалу з таким ідентифікатором {materialId}\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                entity = null;
                Console.WriteLine("Неправильний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return false;
            }
        }
    }
}
