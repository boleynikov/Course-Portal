using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CourseMaterials;
using Services.Helper;

namespace API.View
{
    public class MaterialPageView
    {
        public static void Show(Material material)
        {
            Console.Clear();
            if (material == null)
            {
                Console.WriteLine("Данного ресурсу немає\nНатисніть Enter");
                Console.ReadLine();
                return;
            }

            //Console.WriteLine($"Сторінка матеріалу {material.Title}");

            switch (material.Type)
            {
                case "Article":
                    var article = (ArticleMaterial)material;
                    Console.WriteLine($"Назва статті: {article.Title}\n" +
                                      $"Посилання на статтю: {article.Link}\n" +
                                      $"Дата публікації статті: {article.DateOfPublication}\n");
                    break;
                case "Publication":
                    var publication = (PublicationMaterial)material;
                    Console.WriteLine($"Книга {publication.Title}\n" +
                                      $"За авторством: {publication.Author}\n" +
                                      $"Кількість сторінок: {publication.PageCount}\n");
                    break;
                case "Video":
                    var video = (VideoMaterial)material;
                    Console.WriteLine($"Відео: {video.Title}\n" +
                                      $"Тривалість: {video.Duration}\n" +
                                      $"Якість: {video.Quality}p");
                    break;
            }

            EditNavigationView();
        }

        /// <summary>
        /// Botttom navigation
        /// </summary>
        public static void EditNavigationView()
        {
            Console.WriteLine("Введіть наступні команди:\n" +
                              $"{Command.NextMaterial} - якщо ви переглянули і пройшли цей матеріал\n" +
                              $"{Command.BackToCourse} - якщо ви хочете повернутися на сторінку курсу\n");
        }
    }
}
