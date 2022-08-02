using Data.Repository.Abstract;
using Domain;
using System.Collections.Generic;

namespace Data.Repository
{
    public class CourseRepository : IRepository<Course>
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

        public void DeleteByIndex(int index)
        {
            courses.Remove(courses[index]);
        }

        public Course GetByIndex(int index)
        {
            return courses[index];
        }

        public void Update(Course course)
        {
            //TODO
        }
        public void Save()
        {
            //TODO
        }
    }
}
