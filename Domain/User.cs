using EducationPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain
{
    class User
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        public Dictionary<Skill, int> UserSkills { get; private set; }

        public Dictionary<Course, CourseProgress> UserCourses { get; private set; }
        
        public User(string name, string email)
        {
            Name = name;
            Email = email;
            UserSkills = new Dictionary<Skill, int>();
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
        public void AddSkill(Skill skill, int value)
        {
            if (UserSkills.ContainsKey(skill))
            {
                UserSkills[skill] += value;
            }
            else
            {
                UserSkills.Add(skill, value);
            }
        }
    }
}
