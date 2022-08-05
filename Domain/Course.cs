using Domain.Abstract;
using Domain.CourseMaterials;
using System;
using System.Collections.Generic;

namespace Domain
{
    [Serializable]
    public class Course : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public List<Material> CourseMaterials { get; private set; }

        public List<Skill> CourseSkills { get; private set; }

        public Course(int id, string name, string description) : base(id)
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

        public void Update(string name, string descript, List<Material> materials, List<Skill> skills)
        {
            Name = name;
            Description = descript;
            CourseMaterials = materials;
            CourseSkills = skills;
        }
    }
}
