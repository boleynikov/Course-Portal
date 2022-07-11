using EducationPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Repository
{
    public interface IRepository
    {
        Course Get(int index);
        void Add(Course course);
    }
}
