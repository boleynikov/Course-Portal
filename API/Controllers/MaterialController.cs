using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using API.Controllers.Abstract;
using API.View;
using Domain;
using Domain.CourseMaterials;
using Services.Helper;
using Services.Interface;

namespace API.Controllers
{
    public class MaterialController : IController
    {
        private readonly IAuthorizedUserService _authorizedUser;
        private readonly Course _currentCourse;

        public MaterialController(IAuthorizedUserService authorizedUser, Course currentCourse)
        {
            _authorizedUser = authorizedUser;
            _currentCourse = currentCourse;
        }

        public string Launch()
        {
            var userProgressInCourse = _authorizedUser.Account.UserCourses
                .FirstOrDefault(c => c.Key == _currentCourse.Id).Value.Percentage;
            var progressUnit = 100f / _currentCourse.CourseMaterials.Count;
            var completedMaterials = Convert.ToInt32(userProgressInCourse / progressUnit);

            foreach (var material in _currentCourse.CourseMaterials.Skip(completedMaterials))
            {
                MaterialPageView.Show(material);
                string cmdLine = Console.ReadLine();
                switch (cmdLine)
                {
                    case Command.NextMaterial:
                        var progress = 100f / _currentCourse.CourseMaterials.Count;
                        _authorizedUser.EditCourseProgress(_currentCourse.Id, progress);
                        break;
                    case Command.BackToCourse:
                        return Command.CoursePage;
                }
            }

            return Command.CoursePage;
        }
    }
}
