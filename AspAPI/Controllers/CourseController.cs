using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Services.Interface;

namespace AspAPI.Controllers
{
    public class CourseController : Controller
    {
        private readonly IService<Course> _courseService;
        public CourseController(IService<Course> courseService)
        {
            _courseService = courseService;
        }
        public IActionResult Index(int Id)
        {
            var course = _courseService.GetById(Id);
            return View(course);
        }
    }
}
