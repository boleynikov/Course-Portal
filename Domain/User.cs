// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            UserCourses = new Dictionary<int, CourseProgress>();
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
        /// User courses
        /// </summary>
        public Dictionary<int, CourseProgress> UserCourses { get; private set; }
    }
}
