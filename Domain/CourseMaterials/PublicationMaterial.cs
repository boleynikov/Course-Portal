using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CourseMaterials
{
    public class PublicationMaterial : Material
    {
        public string Author { get; set; }

        public int PageCount { get; set; }

        public string Format { get; set; }

        public DateTime YearOfPublication { get; set; }

        public PublicationMaterial(string title, string author, int pageCount, string format, DateTime yearOfPublication) : base(title)
        {
            Author = author;
            PageCount = pageCount;
            Format = format;
            YearOfPublication = yearOfPublication;
        }
    }
}
