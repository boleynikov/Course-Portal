using Domain.CourseMaterials;
using System.Collections.Generic;

namespace Domain
{
    public class Course
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public List<Material> CourseMaterials { get; private set; }

        public List<Skill> CourseSkills { get; private set; }

        public Course(string name, string description)
        {
            Name = name;
            Description = description;
            CourseMaterials = new List<Material>();
            CourseSkills = new List<Skill>();
        }

        public void AddMaterial(Material material)
        {
            CourseMaterials.Add(material);
        }

        public void AddSkill(Skill skill, int value)
        {
            var skillExist = CourseSkills.Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = CourseSkills.IndexOf(skillExist);
                CourseSkills[index].Points += value;
            }
            else
            {
                CourseSkills.Add(new Skill { Name = skill.Name, Points = value });
            }
        }
    }
}
