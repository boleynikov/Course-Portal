using System;

namespace Domain
{
    [Serializable]
    public class Skill
    {
        public SkillKind Name { get; set; }
        public int Points { get; set; }
    }
    public enum SkillKind
    {
        Programming = 0,
        Music = 1,
        HealthCare = 3,
        TimeManagment = 4,
        Communication = 5,
        Illustration = 6,
        Photo = 7
    }
}
