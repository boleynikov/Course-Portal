using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.CourseMaterials;
using Domain.Enum;

namespace AspAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public CourseStatus Status { get; set; }

        /// <summary>
        /// Gets or set course owner
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets course name.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets course description.
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        /// <summary>
        /// Gets materials in course.
        /// </summary>
        [Display(Name = "Course materials")]
        public ICollection<Material> CourseMaterials { get; set; }

        /// <summary>
        /// Gets skills, that user can get after complete.
        /// </summary>
        [Display(Name = "Course skills")]
        public ICollection<Skill> CourseSkills { get; set; }
    }
}
