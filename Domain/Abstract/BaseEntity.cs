// <copyright file="BaseEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain.Abstract
{
    /// <summary>
    /// Base entity for domain classes.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        /// <param name="id">Entity id.</param>
        protected BaseEntity(int id)
        {
            //Id = id;
        }

        /// <summary>
        /// Gets or sets entity id.
        /// </summary>
        public int Id { get; set; }
    }
}
