using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspAPI.Models.Materials
{
    public class MaterialModel
    {
        /// <summary>
        /// Gets or sets entity id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets type of material.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets title of material.
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets creator of material
        /// </summary>
        public Domain.User User { get; set; }

        /// <summary>
        /// List of attached courses
        /// </summary>
        public ICollection<Domain.Course> Courses { get; set; }
    }
}
