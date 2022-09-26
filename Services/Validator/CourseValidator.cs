using Domain;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Validator
{
    public class CourseValidator : IValidateService<Course>
    {
        public bool Validate(List<Course> Entities, string strEntityId, out Course entity)
        {
            if (int.TryParse(strEntityId, out int courseId))
            {
                try
                {
                    entity = Entities.FirstOrDefault(c => c.Id == courseId)
                        ?? throw new ArgumentOutOfRangeException(nameof(entity));
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    entity = null;
                    Console.WriteLine("Немає курсу з таким ідентифікатором\n" +
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
