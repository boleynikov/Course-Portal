// <copyright file="Course.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.CourseMaterials;

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
        public string Name { get; private set; }

        /// <summary>
        /// Gets course description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets materials in course.
        /// </summary>
        public List<Material> CourseMaterials { get; private set; }

        /// <summary>
        /// Gets skills, that user can get after complete.
        /// </summary>
        public List<Skill> CourseSkills { get; private set; }

        /// <summary>
        /// Adding new skill to course.
        /// </summary>
        /// <param name="skill">New skill.</param>
        /// <param name="value">New skill value.</param>
        public void AddSkill(Skill skill, int value)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            var skillExist = CourseSkills.Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = CourseSkills.IndexOf(skillExist);
                CourseSkills[index].Points += value;
            }
            else
            {
                CourseSkills.Add(new Skill { Name = skill.Name, Points = value });
            }
        }

        /// <summary>
        /// Update Course information.
        /// </summary>
        /// <param name="name">Updated course name.</param>
        /// <param name="descript">Updated course description.</param>
        /// <param name="materials">Updated course materials.</param>
        /// <param name="skills">Updated course skills.</param>
        public void UpdateInfo(string name, string descript, List<Material> materials, List<Skill> skills)
        {
            Name = name;
            Description = descript;
            CourseMaterials = materials;
            CourseSkills = skills;
        }
    }
}
