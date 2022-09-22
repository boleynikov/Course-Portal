using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AspAPI.Extentions
{
    public static class CourseExtention
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
