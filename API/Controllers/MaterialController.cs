﻿using System;
using System.Linq;
using API.Controllers.Abstract;
using API.View;
using Domain;
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
            for (int i = 0; i < _currentCourse.CourseMaterials.Count; i++)
            {
                var userProgressInCourse = _authorizedUser.Account.UserCourses
                    .FirstOrDefault(c => c.Key == _currentCourse.Id).Value.Percentage;
                var progressUnit = 100f / _currentCourse.CourseMaterials.Count;
                var completedMaterials = Convert.ToInt32(userProgressInCourse / progressUnit);

                string cmdLine;
                var material = _currentCourse.CourseMaterials.ElementAt(i);
                if (i < completedMaterials)
                {
                    MaterialPageView.Show(material, _currentCourse.Name, "Пройдено", $"{(i + 1)}/{_currentCourse.CourseMaterials.Count}");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case Command.NextMaterial:
                            continue;
                        case Command.PreviousMaterial:
                            if (i == 0)
                            {
                                Console.WriteLine("Це перший матеріал");
                                Console.ReadLine();
                                i--;
                                break;
                            }

                            i -= 2;
                            break;
                        case Command.BackToCourse:
                            return Command.CoursePage;
                    }
                }
                else
                {
                    MaterialPageView.Show(material, _currentCourse.Name, "Не пройдено", $"{(i + 1)}/{_currentCourse.CourseMaterials.Count}");
                    cmdLine = Console.ReadLine();
                    switch (cmdLine)
                    {
                        case Command.NextMaterial:
                            var progress = 100f / _currentCourse.CourseMaterials.Count;
                            _authorizedUser.EditCourseProgress(_currentCourse.Id, progress);
                            break;
                        case Command.PreviousMaterial:
                            if (i == 0)
                            {
                                Console.WriteLine("Це перший матеріал");
                                Console.ReadLine();
                                i--;
                                break;
                            }

                            i -= 2;
                            break;
                        case Command.BackToCourse:
                            return Command.CoursePage;
                    }
                }
            }

            return Command.CoursePage;
        }
    }
}