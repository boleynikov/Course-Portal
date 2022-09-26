using Domain;
using Domain.CourseMaterials;
using System.Threading.Tasks;

namespace Services.Interface
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IAuthorizedUserService
    {
        /// <summary>
        /// Get current account
        /// </summary>
        /// <returns></returns>
        public User Account { get; set; }
        /// <summary>
        /// Create new course
        /// </summary>
        Task<Course> CreateCourse(string name, string description, string owner, IService<Course> courseService, IService<Material> materialService);
        /// <summary>
        /// Add new course to user list
        /// </summary>
        /// <param name="courseId"></param>
        Task AddCourseToUser(int courseId);

        /// <summary>
        /// Remove course from user list
        /// </summary>
        /// <param name="id"></param>
        void RemoveCourse(int id);

        /// <summary>
        /// Change Course progress in one of user courses
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="percentage"></param>
        void EditCourseProgress(int courseId, float percentage);
        /// <summary>
        /// Add new skill to user
        /// </summary>
        /// <param name="skill"></param>
        void AddSkill(Skill skill);
        /// <summary>
        /// Create Material in command line
        /// </summary>
        /// <returns></returns>
        Task<Material> AddMaterial(IService<Material> materialService, Material material);
    }
}
