using System;

namespace Domain.CourseMaterials
{
    [Serializable]
    public class VideoMaterial : Material
    {
        public float Duration { get; set; }

        public int Quality { get; set; }

        public VideoMaterial(int id, string title, float duration, int quality, string type = "Video") : base(id, title, type)
        {
            Duration = duration;
            Quality = quality;
        }
    }
}
