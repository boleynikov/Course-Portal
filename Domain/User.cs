using Domain.Abstract;
using Domain.CourseMaterials;
using System.Collections.Generic;

namespace Domain
{
    public class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public List<Skill> UserSkills { get; private set; }

        public List<Material> UserMaterials { get; private set; }

        public Dictionary<Course, CourseProgress> UserCourses { get; private set; }
        
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            UserSkills = new List<Skill>();
            UserCourses = new Dictionary<Course, CourseProgress>();
        }

        public void AddCourse(Course course)
        {
            if (UserCourses.ContainsKey(course))
            {
                return;
            }

            UserCourses.Add(course, new CourseProgress() { State = State.NotCompleted, Percentage = 0f }); 
        }
        public void AddSkill(Skill skill)
        {
            var skillExist = UserSkills.Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = UserSkills.IndexOf(skillExist);
                UserSkills[index].Points += skill.Points;
            }
            else
            {
                UserSkills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }
    }
}
