using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AspAPI.Models;
using Domain.CourseMaterials;
using Services.Interface;

namespace AspAPI.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IService<Material> _materialService;
        private readonly IService<Domain.Course> _courseService;

        public MaterialController(IService<Material> materialService, IService<Domain.Course> courseService)
        {
            _materialService = materialService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int courseId, int materialIndex)
        {
            var course = await _courseService.GetById(courseId);
            var material = course.CourseMaterials.ElementAt(materialIndex);
            switch (material.Type)
            {
                case "Article":
                    ViewData["courseId"] = courseId;
                    return View("ArticleIndex", ((ArticleMaterial)material, materialIndex));
                case "Publication":
                    ViewData["courseId"] = courseId;
                    return View("PublicationIndex", ((PublicationMaterial)material, materialIndex));
                case "Video":
                    ViewData["courseId"] = courseId;
                    return View("VideoIndex", ((VideoMaterial)material, materialIndex));
            }

            return View("Error", new ErrorViewModel());
        }

        public IActionResult CreateForm(string option, int id)
        {
            switch (option)
            {
                case "Article":
                    return RedirectToAction("CreateArticleModel", new { id });
                case "Publication":
                    return RedirectToAction("CreatePublicationModel", new { id });
                case "Video":
                    return RedirectToAction("CreateVideoModel", new { id });
            }

            return BadRequest(404);
        }

        public IActionResult CreateArticleModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

        public IActionResult CreatePublicationModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

        public IActionResult CreateVideoModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

    }
}
