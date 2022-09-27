using Domain;
using Domain.CourseMaterials;
using System.Collections.Generic;
using Domain.Enum;

namespace Services.Interface
{
    /// <summary>
    /// Interface of opened course page
    /// </summary>
    public interface IOpenedCourseService
    {
        /// <summary>
        /// Get current opened course
        /// </summary>
        /// <returns></returns>
        Course Get();

        /// <summary>
        /// Add skill to course
        /// </summary>
        void AddOrEditSkill(SkillKind skillName, int skillPoint);
        /// <summary>
        /// Delete skill from course
        /// </summary>
        void DeleteSkill(string skillName);
        /// <summary>
        /// Edit course name
        /// </summary>
        void EditCourseName();
        /// <summary>
        /// Edit course description
        /// </summary>
        void EditCourseDescription();
        /// <summary>
        /// Delete material in opened course
        /// </summary>
        /// <returns>Index of removed material</returns>
        int DeleteCourseMaterial(int id);
        /// <summary>
        /// Add material which user is own to course
        /// </summary>
        void AddCourseMaterial(List<Material> userMaterials);
    }
}
