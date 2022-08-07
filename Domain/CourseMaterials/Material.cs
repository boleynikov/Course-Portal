// <copyright file="Material.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain.CourseMaterials
{
    using Domain.Abstract;

    /// <summary>
    /// Abstract material class.
    /// </summary>
    public abstract class Material : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// Abstract constructor <see cref="Material"/> class.
        /// </summary>
        /// <param name="id">Material id.</param>
        /// <param name="title">Material title.</param>
        /// <param name="type">Type of material.</param>
        protected Material(int id, string title, string type)
            : base(id)
        {
            Title = title;
            Type = type;
        }

        /// <summary>
        /// Gets or sets type of material.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets title of material.
        /// </summary>
        public string Title { get; set; }
    }
}
