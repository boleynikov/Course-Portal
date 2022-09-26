using AspAPI.Models;
using System;

namespace AspAPI.ModelExtension
{
    public static class CourseExtend
    {
        public static Course ToModel(this Domain.Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            return new Course()
            {
                Id = course.Id,
                Name = course.Name,
                Owner = course.Owner,
                Description = course.Description,
                CourseMaterials = course.CourseMaterials,
                CourseSkills = course.CourseSkills,
                Status = course.Status
            };
        }
    }
}
