using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Domain.CourseMaterials
{
    public class ArticleMaterial : Material
    {
        public DateTime DateOfPublication { get; set; }

        public string Link { get; set; }

        public ArticleMaterial(string title, DateTime dateOfPublication, string link): base(title)
        {
            DateOfPublication = dateOfPublication;
            Link = link;
        }
    }
}
