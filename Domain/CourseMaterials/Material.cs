using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CourseMaterials
{
    public abstract class Material : BaseEntity
    {
        public string Title { get; set; }

        protected Material(string title)
        {
            Title = title;
        }
    }
}
