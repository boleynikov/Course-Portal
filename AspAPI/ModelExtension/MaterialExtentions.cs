using AspAPI.Models.Materials;
using Domain.CourseMaterials;
using System;

namespace AspAPI.ModelExtension
{
    public static class MaterialExtend
    {
        public static ArticleMaterial ToDomain(this ArticleModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            return new ArticleMaterial(model.Id, model.Title, model.DateOfPublication, model.Link);
        }
        public static PublicationMaterial ToDomain(this PublicationModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            return new PublicationMaterial(model.Id, model.Title, model.Author, model.PageCount, model.Format, model.YearOfPublication);
        }
        public static VideoMaterial ToDomain(this VideoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            return new VideoMaterial(model.Id, model.Title, model.Duration, model.Quality);
        }
    }
}
