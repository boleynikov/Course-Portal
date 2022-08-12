﻿using Domain;
using Domain.CourseMaterials;
using Services.Interface;
using System;
using System.Collections.Generic;

namespace Services
{
    /// <summary>
    /// Opened Course functionallity
    /// </summary>
    public class OpenedCourseService : IOpenedCourseService
    {
        private Course _course;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenedCourseService"/> class.
        /// </summary>
        /// <param name="course">Course, which will be opened</param>
        public OpenedCourseService(Course course)
        {
            _course = course;
        }

        /// <inheritdoc/>
        public Course Get() => _course;

        /// <inheritdoc/>
        public void AddSkill(Course currentCourse, Skill skill, int value)
        {
            var skills = _course.CourseSkills;
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            var skillExist = skills.Find(c => c.Name == skill.Name);
            if (skillExist != null)
            {
                var index = skills.IndexOf(skillExist);
                skills[index].Points += value;
            }
            else
            {
                skills.Add(new Skill { Name = skill.Name, Points = value });
            }
        }
    }
}
