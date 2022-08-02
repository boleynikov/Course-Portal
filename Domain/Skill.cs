using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
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
