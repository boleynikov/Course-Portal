using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.CourseMaterials;
using Microsoft.AspNetCore.Identity;

namespace AspAPI.Models
{
    public class User : IdentityUser
    {
        [Key]
        public new int Id { get; set; }
        /// <summary>
        /// Gets user Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets user email.
        /// </summary>
        public new string Email { get; set; }

        /// <summary>
        /// Gets user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets user skills.
        /// </summary>
        public ICollection<Skill> UserSkills { get; set; }

        /// <summary>
        /// Gets user materials.
        /// </summary>
        public ICollection<Material> UserMaterials { get; set; }

        /// <summary>
        /// User courses
        /// </summary>
        public Dictionary<int, CourseProgress> UserCourses { get; set; }
    }
}
