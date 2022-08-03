using Domain;
using Domain.CourseMaterials;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController
    {
        private readonly IService<Course> courseService;
        private readonly IService<Material> materialService;

        public UserController(IService<Course> service, IService<Material> service1)
        {
            courseService = service;
            materialService = service1;
        }

        public void Launch()
        {

        }
    }
}
