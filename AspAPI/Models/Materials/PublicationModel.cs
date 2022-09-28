using System;
using System.ComponentModel.DataAnnotations;

namespace AspAPI.Models.Materials
{
    public class PublicationModel : MaterialModel
    {
        /// <summary>
        /// Gets or sets publication author.
        /// </summary>
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets publication count of pages.
        /// </summary>
        [Display(Name = "Page count")]
        [Required(ErrorMessage = "Author is required")]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets format of publication.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets year of publication(release).
        /// </summary>
        [Display(Name = "Publication")]
        [Required(ErrorMessage = "Date of publication is required")]
        public DateTime YearOfPublication { get; set; }

        /// <summary>
        /// Gets or sets type of material.
        /// </summary>
        public string Type { get; set; } = "Publication";
    }
}
