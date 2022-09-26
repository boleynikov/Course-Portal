// <copyright file="CourseProgress.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain
{
    using Enum;
    using System;

    /// <summary>
    /// Course Progress class.
    /// </summary>
    [Serializable]
    public class CourseProgress
    {
        /// <summary>
        /// Gets or sets state of course progress.
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Gets or sets percentage of course progress.
        /// </summary>
        public float Percentage { get; set; }
    }
}
