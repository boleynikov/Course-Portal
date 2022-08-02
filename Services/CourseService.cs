using Data.Repository.Abstract;
using Domain;
using Services.Abstract;

namespace EducationPortal.Services
{
    public class CourseService : IService<Course>
    {
        private readonly IRepository<Course> repository;

        public CourseService(IRepository<Course> repository)
        {
            this.repository = repository;
        }
        public void Add(Course course)
        {
            repository.Add(course);
        }

        public void DeleteByIndex(int index)
        {
            repository.DeleteByIndex(index);
        }

        public Course GetByIndex(int index)
        {
            return repository.GetByIndex(index);
        }

        public void Save()
        {
            repository.Save();
        }

        public void Update(Course course)
        {
            repository.Update(course);
        }
    }
}
