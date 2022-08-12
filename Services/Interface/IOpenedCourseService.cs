﻿using Domain;
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
        void DeleteCourseMaterial();
        /// <summary>
        /// Add material which user is own to course
        /// </summary>
        void AddCourseMaterial(List<Material> userMaterials);
        /// <summary>
        /// Input string and convert to valid id of user material
        /// </summary>
        /// <param name="strMaterialId"></param>
        /// <param name="material"></param>
        /// <param name="materialAttachedTo"></param>
        /// <returns></returns>
        bool ValidateMaterial(string strMaterialId, out Material material, List<Material> userMaterials);
    }
}
