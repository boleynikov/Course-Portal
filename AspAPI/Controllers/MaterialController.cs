using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public MaterialController(IService<Material> materialService)
        {
            _materialService = materialService;
        }

        // GET: MaterialController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaterialController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MaterialController/CreateForm
        public ActionResult CreateForm(string option, int id)
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

        public ActionResult CreateArticleModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

        public ActionResult CreatePublicationModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }

        public ActionResult CreateVideoModel(int id)
        {
            ViewData["courseId"] = id;
            return View();
        }



        // GET: MaterialController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
