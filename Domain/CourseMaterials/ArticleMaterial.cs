// <copyright file="ArticleMaterial.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Domain.CourseMaterials
{
    using System;

    /// <summary>
    /// Article material.
    /// </summary>
    [Serializable]
    public class ArticleMaterial : Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleMaterial"/> class.
        /// </summary>
        /// <param name="id">Material id.</param>
        /// <param name="title">Material title.</param>
        /// <param name="dateOfPublication">Publication date of article.</param>
        /// <param name="link">Link on Article in web.</param>
        /// <param name="type">Type of material.</param>
        public ArticleMaterial(int id, string title, DateTime dateOfPublication, string link, string type = "Article")
            : base(id, title, type)
        {
            DateOfPublication = dateOfPublication;
            Link = link;
        }

        /// <summary>
        /// Gets or sets publication date of article.
        /// </summary>
        [Display(Name = "Release date")]
        public DateTime DateOfPublication { get; set; }

        /// <summary>
        /// Gets or sets link on Article in web.
        /// </summary>
        public string Link { get; set; }
    }
}
