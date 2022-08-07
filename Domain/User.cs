// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.CourseMaterials;

    /// <summary>
    /// User class.
    /// </summary>
    [Serializable]
    public class User : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="name">User name.</param>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        public User(int id, string name, string email, string password)
            : base(id)
        {
            Name = name;
            Email = email;
            Password = password;
            UserSkills = new List<Skill>();
            UserMaterials = new List<Material>();
            UserCourses = new List<(Course, CourseProgress)>();
        }

        /// <summary>
        /// Gets user Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets user email.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets user password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets user skills.
        /// </summary>
        public List<Skill> UserSkills { get; private set; }

        /// <summary>
        /// Gets user materials.
        /// </summary>
        public List<Material> UserMaterials { get; private set; }

        /// <summary>
        /// Gets user courses.
        /// </summary>
        public List<(Course, CourseProgress)> UserCourses { get; private set; }

        /// <summary>
        /// Add Material to user.
        /// </summary>
        /// <param name="material">Added material.</param>
        public void AddMaterial(Material material)
        {
            UserMaterials.Add(material);
        }

        /// <summary>
        /// Update existing user course.
        /// </summary>
        /// <param name="editedCourse">Updated course.</param>
        public void UpdateCourseInfo(Course editedCourse)
        {
            if (editedCourse == null)
            {
                throw new ArgumentNullException(nameof(editedCourse));
            }

            var pulledUserCourse = UserCourses.Find(course => course.Item1.Id == editedCourse.Id).Item1;
            if (pulledUserCourse == null)
            {
                return;
            }

            var pulledProgress = UserCourses.Find(course => course.Item1.Id == editedCourse.Id).Item2;
            var index = UserCourses.IndexOf((pulledUserCourse, pulledProgress));
            UserCourses[index] = (editedCourse, pulledProgress);
        }

        /// <summary>
        /// Adding new course.
        /// </summary>
        /// <param name="newCourse">New Course.</param>
        public void AddCourse(Course newCourse)
        {
            if (newCourse == null)
            {
                throw new ArgumentNullException(nameof(newCourse));
            }

            if (UserCourses.Find(course => course.Item1.Name == newCourse.Name &&
                                           course.Item1.Description == newCourse.Description).Item1 != null)
            {
                return;
            }

            UserCourses.Add((newCourse, new CourseProgress() { State = State.NotCompleted, Percentage = 0f }));
        }

        /// <summary>
        /// Remove course from user catalog.
        /// </summary>
        /// <param name="id">Course id.</param>
        public void RemoveCourse(int id)
        {
            var pulledCourse = UserCourses.Find(course => course.Item1.Id == id).Item1;
            if (pulledCourse == null)
            {
                return;
            }

            var pulledProgress = UserCourses.Find(course => course.Item1.Id == id).Item2;
            UserCourses.Remove((pulledCourse, pulledProgress));
        }

        /// <summary>
        /// Adding new skill to user.
        /// </summary>
        /// <param name="skill">New skill.</param>
        public void AddSkill(Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            if (UserSkills.Find(c => c.Name == skill.Name) != null)
            {
                var index = UserSkills.IndexOf(UserSkills.Find(c => c.Name == skill.Name));
                UserSkills[index].Points += skill.Points;
            }
            else
            {
                UserSkills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }
    }
}
