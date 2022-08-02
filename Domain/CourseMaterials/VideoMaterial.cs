using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CourseMaterials
{
    class VideoMaterial : Material
    {
        public float Duration { get; set; }

        public int Quality { get; set; }

        public VideoMaterial(string title, float duration, int quality) : base(title)
        {
            Duration = duration;
            Quality = quality;
        }
    }
}
