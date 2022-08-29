using Domain;
using Domain.CourseMaterials;
using System.Collections.Generic;

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
        Course CreateCourse(IService<Course> courseService, IService<Material> materialService);
        /// <summary>
        /// Add new course to user list
        /// </summary>
        /// <param name="newCourse"></param>
        void AddCourse(Course newCourse);

        /// <summary>
        /// Remove course from user list
        /// </summary>
        /// <param name="id"></param>
        void RemoveCourse(int id);

        /// <summary>
        /// Add new skill to user
        /// </summary>
        /// <param name="skill"></param>
        void AddSkill(Skill skill);
        /// <summary>
        /// Create skill from command line
        /// </summary>
        /// <param name="cmdLine"></param>
        /// <returns></returns>
        Skill CreateSkill(string cmdLine);
        /// <summary>
        /// Create Material in command line
        /// </summary>
        /// <returns></returns>
        IEnumerable<Material> CreateMaterials(IService<Material> materialService);
        /// <summary>
        /// Add exist user material to course
        /// </summary>
        /// <returns></returns>
        IEnumerable<Material> AddExistingMaterials();
    }
}
