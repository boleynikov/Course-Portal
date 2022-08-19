// <copyright file="Course.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using CourseMaterials;

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
        public Course(int id, string name, string description)
            : base(id)
        {
            Name = name;
            Description = description;
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

        /// <summary>
        /// Gets materials in course.
        /// </summary>
        public List<Material> CourseMaterials { get; private set; }

        /// <summary>
        /// Gets skills, that user can get after complete.
        /// </summary>
        public List<Skill> CourseSkills { get; private set; }
    }
}
