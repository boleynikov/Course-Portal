// <copyright file="VideoMaterial.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Domain.CourseMaterials
{
    using System;

    /// <summary>
    /// Video Material.
    /// </summary>
    [Serializable]
    public class VideoMaterial : Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMaterial"/> class.
        /// </summary>
        /// <param name="id">Material id.</param>
        /// <param name="title">Material title.</param>
        /// <param name="duration">Video duration.</param>
        /// <param name="quality">Video quality.</param>
        /// <param name="type">Type of material.</param>
        public VideoMaterial(int id, string title, float duration, int quality, string type = "Video")
            : base(id, title, type)
        {
            Duration = duration;
            Quality = quality;
        }

        /// <summary>
        /// Gets or sets duration of Video.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Gets or sets quality of Video.
        /// </summary>
        public int Quality { get; set; }
    }
}
