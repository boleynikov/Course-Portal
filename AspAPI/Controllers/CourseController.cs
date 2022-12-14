using AspAPI.Mapper;
using AspAPI.Models.Materials;
using Domain;
using Domain.CourseMaterials;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspAPI.Controllers
{
    public class CourseController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Course> _courseService;
        private readonly IService<Material> _materialService;
        private readonly IAuthorizedUserService _authorizedUser;
        public CourseController(
            IService<User> userService, 
            IService<Course> courseService,
            IService<Material> materialService,
            IAuthorizedUserService authorizedUser)
        {
            _userService = userService;
            _courseService = courseService;
            _materialService = materialService;
            _authorizedUser = authorizedUser;
        }
        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetById(id);

            return PartialView("_CourseDetailPage", course);
        }

        public async Task<IActionResult> CreateCourse(Models.Course createdCourse)
        {
            if (ModelState.IsValid)
            {
                var course = await _authorizedUser.CreateCourse(createdCourse?.Name, createdCourse.Description, _authorizedUser.Account.Email, _courseService, _materialService);
                await _courseService.Add(course);
                await _authorizedUser.AddCourseToUser(course.Id);
                await _userService.Save();
                return RedirectToAction("UserProfile", "Identity");
            }

            return RedirectToAction("CreateCourseForm", "Identity");
        }

        public async Task<IActionResult> AddCourseToUser(int courseId)
        {
            await _authorizedUser.AddCourseToUser(courseId);
            await _userService.Save();
            return RedirectToAction("Index", "Material", new { courseId = courseId, materialId = 0 });
        }

        public async Task<IActionResult> AddUserProgress(int courseId, int materialIndex)
        {
            var course = await _courseService.GetById(courseId);
            var userProgressInCourse = _authorizedUser.Account.UserCourses
                .FirstOrDefault(c => c.Key == course.Id).Value.Percentage;
            var progressUnit = 100f / course.CourseMaterials.Count;
            var completedMaterials = Convert.ToInt32(userProgressInCourse / progressUnit);
            if (materialIndex >= completedMaterials)
            {
                _authorizedUser.EditCourseProgress(course.Id, progressUnit);
            }

            if (_authorizedUser.Account.UserCourses[course.Id].State == State.PreCompleted)
            {
                foreach (var courseSkill in course.CourseSkills)
                {
                    _authorizedUser.AddSkill(courseSkill);
                }

                _authorizedUser.Account.UserCourses[course.Id].State = State.Completed;
            }

            await _userService.Update(_authorizedUser.Account);
            return RedirectToAction("Index", "Material", new { courseId, materialIndex});
        }

        public async Task<IActionResult> RemoveCourseFromUser(int courseId)
        {
            _authorizedUser.RemoveCourse(courseId);
            await _userService.Update(_authorizedUser.Account);
            return RedirectToAction("UserProfile","Identity");
        }

        public async Task<IActionResult> EditForm(int id)
        {
            var course = await _courseService.GetById(id);
            var courseModel = OwnMapper.Map<Course, Models.Course>(course);
            return View(courseModel);
        }

        public async Task<IActionResult> SaveNameDescription(int courseId, Models.Course model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Name != null && model.Description != null) 
            {
                var course = await _courseService.GetById(courseId);
                course.Name = model.Name;
                course.Description = model.Description;
                course.Status = model.Status;
                await _courseService.Update(course);
                var courseModel = OwnMapper.Map<Course, Models.Course>(course);
                return View("EditForm", courseModel);
            }

            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> RemoveMaterialFromCourse(int courseId, int materialId)
        {
            var course = await _courseService.GetById(courseId);
            var material = course.CourseMaterials.FirstOrDefault(m => m.Id == materialId);
            if (material == null)
            {
                throw new ArgumentOutOfRangeException(nameof(material));
            }

            course.CourseMaterials.Remove(material);
            await _courseService.Update(course);
            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> AddSkillToCourse(int courseId, string skillName, int skillPoint)
        {
            var skill = new Skill() { Name = Enum.Parse<SkillKind>(skillName), Points = skillPoint };
            var course = await _courseService.GetById(courseId);
            var existingSkill = course.CourseSkills.ToList().Find(c => c.Name == skill.Name);

            if (existingSkill != null)
            {
                var index = course.CourseSkills.ToList().IndexOf(existingSkill);
                course.CourseSkills.ElementAt(index).Points += skill.Points;
            }
            else
            {
                course.CourseSkills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }

            course.Status = CourseStatus.Edited;
            await _courseService.Update(course);
            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> RemoveSkillFromCourse(int courseId, string skillName)
        {
            var course = await _courseService.GetById(courseId);
            var skill = course.CourseSkills.ToList().Find(s => s.Name == Enum.Parse<SkillKind>(skillName));
            course.CourseSkills.Remove(skill);
            course.Status = CourseStatus.Edited;
            await _courseService.Update(course);
            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> AddArticleToCourse(ArticleModel articleModel, int courseId)
        {
            var article = OwnMapper.Map<ArticleModel, ArticleMaterial>(articleModel);
            var material = await _authorizedUser.AddMaterial(_materialService, article);
            var course = await _courseService.GetById(courseId);
            course.CourseMaterials.Add(material);
            course.Status = CourseStatus.Edited;
            await _courseService.Save();
            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> AddPublicationToCourse(PublicationModel publicationModel, int courseId)
        {
            var publication = OwnMapper.Map<PublicationModel, PublicationMaterial>(publicationModel);
            var material = await _authorizedUser.AddMaterial(_materialService, publication);
            var course = await _courseService.GetById(courseId);
            course.CourseMaterials.Add(material);
            course.Status = CourseStatus.Edited;
            await _courseService.Save();
            return RedirectToAction("EditForm", new { id = courseId });
        }

        public async Task<IActionResult> AddVideoToCourse(VideoModel videoModel, int courseId)
        {
            var video = OwnMapper.Map<VideoModel, VideoMaterial>(videoModel);
            var material = await _authorizedUser.AddMaterial(_materialService, video);
            var course = await _courseService.GetById(courseId);
            course.CourseMaterials.Add(material);
            course.Status = CourseStatus.Edited;
            await _courseService.Save();
            return RedirectToAction("EditForm", new { id = courseId });
        }
    }
}
