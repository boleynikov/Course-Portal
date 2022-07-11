using EducationPortal.Domain.CourseMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain
{
    public class Course
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public List<Material> CourseMaterials { get; private set; }

        public Dictionary<Skill, int> CourseSkills { get; private set; }

        public Course(string name, string description)
        {
            Name = name;
            Description = description;
            CourseMaterials = new List<Material>();
            CourseSkills = new Dictionary<Skill, int>();
        }

        public void AddMaterial(Material material)
        {
            CourseMaterials.Add(material);
        }

        public void AddSkill(Skill skill, int value)
        {
            if (CourseSkills.ContainsKey(skill))
            {
                CourseSkills[skill]+=value;
            }
            else
            {
                CourseSkills.Add(skill, value);
            }
        }
    }
}
