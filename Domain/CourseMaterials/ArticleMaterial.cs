using System;

namespace Domain.CourseMaterials
{
    [Serializable]
    public class ArticleMaterial : Material
    {
        public DateTime DateOfPublication { get; set; }

        public string Link { get; set; }

        public ArticleMaterial(int id, string title, DateTime dateOfPublication, string link, string type = "Article"): base(id, title, type)
        {
            DateOfPublication = dateOfPublication;
            Link = link;
        }
    }
}
