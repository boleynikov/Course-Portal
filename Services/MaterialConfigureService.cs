using Domain;
using Domain.CourseMaterials;
using Services.Helper;
using Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class MaterialConfigureService
    {
        public async Task<Material> CreateMaterials(IService<Material> materialService, User account)
        {
            if (materialService == null)
            {
                throw new ArgumentNullException(nameof(materialService));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var allMaterials = await materialService.GetAll(0);
            Console.Write("Обраний матеріал: ");
            var cmdLine = UserInput.NotEmptyString(() => Console.ReadLine());
            var materialsCount = await materialService.GetCount();
            int id;
            if (materialsCount == 0)
            {
                id = 1;
            }
            else
            {
                id = allMaterials.ToList()[materialsCount - 1].Id + 1;
            }

            switch (cmdLine)
            {
                case Command.ArticleInputCase:
                    var article = ArticleUserInput(id);
                    account.UserMaterials.Add(article);
                    await materialService.Add(article);
                    return article;
                case Command.PublicationInputCase:
                    var publication = PublicationUserInput(id);
                    account.UserMaterials.Add(publication);
                    await materialService.Add(publication);
                    return publication;
                case Command.VideoInputCase:
                    var video = VideoUserInput(id);
                    account.UserMaterials.Add(video);
                    await materialService.Add(video);
                    return video;
                default:
                    Console.WriteLine("На жаль це не тип матеріалу\n" +
                                      "Натисніть Enter");
                    Console.ReadLine();
                    return null;
            }
        }


        private static ArticleMaterial ArticleUserInput(int id)
        {
            Console.Write("Введіть назву статті: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть дату публікації статті: ");
            DateTime date = UserInput.ValidDateTime(() => Console.ReadLine());
            Console.Write("Введіть посиланя на статтю: ");
            string link = UserInput.NotEmptyString(() => Console.ReadLine());

            return new ArticleMaterial(id, title, date, link);
        }
        private static PublicationMaterial PublicationUserInput(int id)
        {
            Console.Write("Введіть назву публікації: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть автора публікації: ");
            string author = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть кількість сторінок публікації: ");
            int pageCount = UserInput.ValidInt(() => Console.ReadLine());
            Console.Write("Введіть формат файлу публікації: ");
            string format = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть дату публікації: ");
            DateTime date = UserInput.ValidDateTime(() => Console.ReadLine());

            return new PublicationMaterial(id, title, author, pageCount, format, date);
        }

        private static VideoMaterial VideoUserInput(int id)
        {
            Console.Write("Введіть назву відео: ");
            string title = UserInput.NotEmptyString(() => Console.ReadLine());
            Console.Write("Введіть довжину відео: ");
            float duration = UserInput.ValidFloat(() => Console.ReadLine());
            Console.Write("Введіть якість відео: ");
            int quality = UserInput.ValidInt(() => Console.ReadLine());

            return new VideoMaterial(id, title, duration, quality);
        }

    }
}
