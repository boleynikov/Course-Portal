using AspAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Services.Interface;
using Course = Domain.Course;

namespace AspAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IService<Course> _courseService;
        public HomeController(ILogger<HomeController> logger, IService<Course> courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            IEnumerable<Course> courses;
            if (searchString != null)
            {
                var result = await _courseService.GetAll(0);
                courses = result.Where(course => course.Name.StartsWith(searchString, true, CultureInfo.InvariantCulture))
                    .ToList();
            }
            else
            {
                courses = await _courseService.GetAll(page);
            }

            var pageCount = await _courseService.GetCount();
            pageCount = pageCount % 6 == 0 ? pageCount / 6 : (pageCount / 6) + 1;
            ViewData["pageCount"] = pageCount;
            return View((courses, page));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
