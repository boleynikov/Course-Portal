using System;
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
        private readonly IAuthorizedUserService _authorizedUser;

        public MaterialController(IService<Material> materialService, IService<Domain.Course> courseService, IAuthorizedUserService authorizedUser)
        {
            _materialService = materialService;
            _courseService = courseService;
            _authorizedUser = authorizedUser;
        }

        public async Task<IActionResult> Index(int courseId, int materialIndex)
        {
            var course = await _courseService.GetById(courseId);
            var material = course.CourseMaterials.ElementAt(materialIndex);
            var userProgressInCourse = _authorizedUser.Account.UserCourses
                .FirstOrDefault(c => c.Key == course.Id).Value.Percentage;
            var progressUnit = 100f / course.CourseMaterials.Count;
            var completedMaterials = Convert.ToInt32(userProgressInCourse / progressUnit);
            switch (material.Type)
            {
                case "Article":
                    ViewData["courseId"] = courseId;
                    return View("ArticleIndex", ((ArticleMaterial)material, materialIndex, completedMaterials));
                case "Publication":
                    ViewData["courseId"] = courseId;
                    return View("PublicationIndex", ((PublicationMaterial)material, materialIndex, completedMaterials));
                case "Video":
                    ViewData["courseId"] = courseId;
                    return View("VideoIndex", ((VideoMaterial)material, materialIndex, completedMaterials));
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
