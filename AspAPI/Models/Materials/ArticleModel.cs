﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AspAPI.Models.Materials
{
    public class ArticleModel : MaterialModel
    {
        /// <summary>
        /// Gets or sets link on Article in web.
        /// </summary>
        [Required(ErrorMessage = "Link is required")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets publication date of article.
        /// </summary>
        [Display(Name = "Publication")]
        [Required(ErrorMessage = "Date of publication is required")]
        public DateTime DateOfPublication { get; set; }
    }
}
