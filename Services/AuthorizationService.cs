// <copyright file="AuthorizationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Domain;
    using Domain.Enum;
    using Services.Interface;

    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IService<User> _userService;
        private User _account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
        /// </summary>
        /// <param name="service">User service for decorating.</param>
        public AuthorizationService(IService<User> service)
        {
            _userService = service;
        }

        /// <inheritdoc/>
        public void AddCourse(Course newCourse)
        {
            if (newCourse == null)
            {
                throw new ArgumentNullException(nameof(newCourse));
            }

            if (_account.UserCourses.Find(course => course.Course.Name == newCourse.Name &&
                                           course.Course.Description == newCourse.Description).Course != null)
            {
                Console.WriteLine("Даний курс у вас уже є\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return;
            }

            _account.UserCourses.Add((newCourse, new CourseProgress() { State = State.NotCompleted, Percentage = 0f }));
        }

        /// <inheritdoc/>
        public void AddSkill(Skill skill)
        {
            var skills = _account.UserSkills.ToList();
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            if (skills.Find(c => c.Name == skill.Name) != null)
            {
                var index = skills.IndexOf(skills.Find(c => c.Name == skill.Name));
                skills[index].Points += skill.Points;
            }
            else
            {
                skills.Add(new Skill { Name = skill.Name, Points = skill.Points });
            }
        }

        /// <inheritdoc/>
        public User Get() => _account;

        /// <inheritdoc/>
        public void Login(string email, string password)
        {
            var loginResult = TryLogin(email, password);
            if (loginResult)
            {
                Console.WriteLine($"З поверненням {_account.Name}\n" +
                                   "Натисніть Enter");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Невірний email чи пароль");
                Console.ReadLine();
            }
        }

        /// <inheritdoc/>
        public void Logout()
        {
            _account = null;
        }

        /// <inheritdoc/>
        public void Register(string name, string email, string password)
        {
            TryRegister(name, email, password);
        }

        /// <inheritdoc/>
        public void RemoveCourse(int id)
        {
            var courses = _account.UserCourses;
            var pulledCourse = courses.Find(course => course.Course.Id == id).Course;
            if (pulledCourse == null)
            {
                return;
            }

            var pulledProgress = courses.Find(course => course.Course.Id == id).Progress;
            courses.Remove((pulledCourse, pulledProgress));
        }

        /// <inheritdoc/>
        public void UpdateCourseInfo(Course editedCourse)
        {
            var courses = _account.UserCourses;
            if (editedCourse == null)
            {
                throw new ArgumentNullException(nameof(editedCourse));
            }

            var pulledUserCourse = courses.Find(course => course.Course.Id == editedCourse.Id).Course;
            if (pulledUserCourse == null)
            {
                return;
            }

            var pulledProgress = courses.Find(course => course.Course.Id == editedCourse.Id).Progress;
            var index = courses.IndexOf((pulledUserCourse, pulledProgress));
            courses[index] = (editedCourse, pulledProgress);
        }

        /// <inheritdoc/>
        public bool ValidateCourse(IService<Course> courseService, string strCourseId, out Course course)
        {
            if (courseService == null)
            {
                throw new ArgumentNullException(nameof(courseService));
            }

            if (int.TryParse(strCourseId, out int courseId))
            {
                try
                {
                    course = courseService.GetById(courseId);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    course = null;
                    Console.WriteLine($"Немає курсу з таким ідентифікатором {courseId}\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                course = null;
                Console.WriteLine("Неправильний формат вводу\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                static string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool TryLogin(string email, string password)
        {
            var allUsers = _userService.GetAll();
            var pulledUser = allUsers.SingleOrDefault(user => user.Email == email);
            if (pulledUser != null)
            {
                if (password == pulledUser.Password)
                {
                    _account = pulledUser;
                    return true;
                }
            }

            return false;
        }

        private void TryRegister(string name, string email, string password)
        {
            if (IsValidEmail(email))
            {
                var id = _userService.GetAll().ToList().Count + 1;
                var newUser = new User(id, name, email, password);
                _userService.Add(newUser);
                _account = newUser;
            }
            else
            {
                Console.WriteLine("E-mail у неправильному форматі\n" +
                                  "Натисніть Enter");
                Console.ReadLine();
            }
        }
    }
}
