using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspAPI.Models.Materials
{
    public class VideoModel : MaterialModel
    {
        /// <summary>
        /// Gets or sets duration of Video.
        /// </summary>
        [Required(ErrorMessage = "Duration is required")]
        public float Duration { get; set; }

        /// <summary>
        /// Gets or sets quality of Video.
        /// </summary>
        [Required(ErrorMessage = "Quality is required")]
        public int Quality { get; set; }
    }
}
