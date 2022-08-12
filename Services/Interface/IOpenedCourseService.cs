using Domain;
using Domain.CourseMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="skill"></param>
        /// <param name="value"></param>
        void AddSkill(Course currentCourse, Skill skill, int value);
    }
}
