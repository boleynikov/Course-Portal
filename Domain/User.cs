using Domain.Abstract;
using Domain.CourseMaterials;
using System;
using System.Collections.Generic;

namespace Domain
{
    [Serializable]
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public List<Skill> UserSkills { get; private set; }

        public List<Material> UserMaterials { get; private set; }

        public List<(Course, CourseProgress)> UserCourses { get; private set; }
        
        public User(int id, string name, string email, string password) : base(id)
        {
            Name = name;
            Email = email;
            Password = password;
            UserSkills = new List<Skill>();
            UserMaterials = new List<Material>();
            UserCourses = new List<(Course, CourseProgress)>();
        }
        public void AddMaterial(Material material)
        {        
            UserMaterials.Add(material);
        }
        
        public void AddCourse(Course newCourse)
        {
            if (UserCourses.Find(course => course.Item1.Name == newCourse.Name && course.Item1.Description == newCourse.Description).Item1 != null)
            {
                return;
            }
            UserCourses.Add((newCourse, new CourseProgress() { State = State.NotCompleted, Percentage = 0f }));
        }

        public void RemoveCourse(int id)
        {
            var pulledCourse = UserCourses.Find(course => course.Item1.Id == id).Item1;
            if (pulledCourse == null)
            {
                return;
            }
            var pulledProgress = UserCourses.Find(course => course.Item1.Id == id).Item2;
            var result = UserCourses.Remove((pulledCourse, pulledProgress));
        }
        public void AddSkill(Skill skill)
        {
            
            if (UserSkills.Find(c => c.Name == skill.Name) != null)
            {
                var index = UserSkills.IndexOf(UserSkills.Find(c => c.Name == skill.Name));
                UserSkills[index].Points += skill.Points;
            }
            else
            {
                UserSkills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }
    }
}
