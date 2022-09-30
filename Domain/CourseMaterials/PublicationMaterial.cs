// <copyright file="PublicationMaterial.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Domain.CourseMaterials
{
    using System;

    /// <summary>
    /// Publication material.
    /// </summary>
    [Serializable]
    public class PublicationMaterial : Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationMaterial"/> class.
        /// </summary>
        /// <param name="id">Material id.</param>
        /// <param name="title">Material title.</param>
        /// <param name="author">Author of publication.</param>
        /// <param name="pageCount">PageCount of publication.</param>
        /// <param name="format">Format of publication.</param>
        /// <param name="yearOfPublication">Year of publication.</param>
        /// <param name="type">Type of material.</param>
        public PublicationMaterial(int id, string title, string author, int pageCount, string format, DateTime yearOfPublication, string type = "Publication")
            : base(id, title, type)
        {
            Author = author;
            PageCount = pageCount;
            Format = format;
            YearOfPublication = yearOfPublication;
        }

        /// <summary>
        /// Gets or sets publication author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets publication count of pages.
        /// </summary>
        [Display(Name = "Page count")]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets format of publication.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets year of publication(release).
        /// </summary>
        [Display(Name = "Release year")]
        public DateTime YearOfPublication { get; set; }
    }
}
