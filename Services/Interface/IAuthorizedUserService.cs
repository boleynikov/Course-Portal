using Domain;

namespace Services.Interface
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IAuthorizedUserService
    {
        /// <summary>
        /// Update existing course in user list
        /// </summary>
        /// <param name="editedCourse"></param>
        void UpdateCourseInfo(Course editedCourse);
        /// <summary>
        /// Valid user input to course id
        /// </summary>
        /// <param name="courseSrevice"></param>
        /// <param name="strCourseId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        bool ValidateCourse(IService<Course> courseSrevice, string strCourseId, out Course course);
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
    }
}
