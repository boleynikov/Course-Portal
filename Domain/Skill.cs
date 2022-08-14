// <copyright file="Skill.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using Domain.Enum;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Skill class.
    /// </summary>
    [Serializable]
    public class Skill
    {
        /// <summary>
        /// Gets or sets name of skill.
        /// </summary>
        [Key]
        public SkillKind Name { get; set; }

        /// <summary>
        /// Gets or sets level of current skill.
        /// </summary>
        public int Points { get; set; }
    }
}
