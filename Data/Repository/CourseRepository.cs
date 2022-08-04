using Data.Repository.Abstract;
using Domain;
using System.Collections.Generic;

namespace Data.Repository
{
    public class CourseRepository : IRepository<Course>
    {
        private readonly IDbContext dbContext;
        private readonly List<Course> courses;

        public CourseRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
            courses = dbContext.Get<Course>();
        }

        public void Add(Course course)
        {
            courses.Add(course);
            Save();
        }

        public void DeleteByIndex(int index)
        {
            courses.Remove(courses[index]);
            Save();
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
            dbContext.Update(courses);
        }

        public Course[] GetAll()
        {
            return courses.ToArray();
        }
    }
}
