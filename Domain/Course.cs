// <copyright file="Course.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using CourseMaterials;
    using Enum;

    /// <summary>
    /// Course.
    /// </summary>
    [Serializable]
    public class Course : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <param name="name">Course name.</param>
        /// <param name="description">Course description.</param>
        /// <param name ="status">Editing course status</param>
        public Course(int id, string name, string description, CourseStatus status = CourseStatus.Unultered)
            : base(id)
        {
            Name = name;
            Description = description;
            Status = status;
            CourseMaterials = new List<Material>();
            CourseSkills = new List<Skill>();
        }

        /// <summary>
        /// Gets course name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets course description.
        /// </summary>
        public string Description { get; set; }

        public CourseStatus Status { get; set; }

        /// <summary>
        /// Gets materials in course.
        /// </summary>
        [Display(Name = "Course materials")]
        public ICollection<Material> CourseMaterials { get; private set; }

        /// <summary>
        /// Gets skills, that user can get after complete.
        /// </summary>
        [Display(Name = "Course skills")]
        public ICollection<Skill> CourseSkills { get; private set; }
    }
}
