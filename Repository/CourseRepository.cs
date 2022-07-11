using EducationPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Repository
{
    public class CourseRepository : IRepository
    {
        private readonly List<Course> courses;

        public CourseRepository()
        {
            courses = new List<Course>();
        }

        public void Add(Course course)
        {
            courses.Add(course);
        }

        public Course Get(int index)
        {
            return courses[index];
        }
    }
}
