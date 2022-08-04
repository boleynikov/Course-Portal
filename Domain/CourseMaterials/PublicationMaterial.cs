using System;

namespace Domain.CourseMaterials
{
    [Serializable]
    public class PublicationMaterial : Material
    {
        public string Author { get; set; }

        public int PageCount { get; set; }

        public string Format { get; set; }

        public DateTime YearOfPublication { get; set; }

        public PublicationMaterial(int id, string title, string author, int pageCount, string format, DateTime yearOfPublication, string type = "Publication") : base(id, title, type)
        {
            Author = author;
            PageCount = pageCount;
            Format = format;
            YearOfPublication = yearOfPublication;
        }
    }
}
