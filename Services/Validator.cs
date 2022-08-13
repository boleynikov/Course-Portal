using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Validator
    {
        public CourseValidator Course;
        public MaterialsValidator Material;
        public Validator()
        {
            Course = new CourseValidator();
            Material = new MaterialsValidator();
        }
    }
}
