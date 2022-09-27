using System;
using Domain.CourseMaterials;
using Services.Helper;

namespace API.View
{
    public static class MaterialPageView
    {
        /// <summary>
        /// Show material page to console
        /// </summary>
        /// <param name="material">Current material</param>
        /// <param name="courseName">Name of current course</param>
        /// <param name="stageStatus">Status of completed material by user</param>
        /// <param name="pageCount">Summary materials count in course</param>
        public static void Show(Material material, string courseName, string stageStatus, string pageCount)
        {
            Console.Clear();
            if (material == null)
            {
                Console.WriteLine("Данного ресурсу немає\nНатисніть Enter");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Курс {courseName}. {pageCount}\n");

            switch (material.Type)
            {
                case "Article":
                    var article = (ArticleMaterial)material;
                    Console.WriteLine($"Стаття: {article.Title} | {stageStatus}\n" +
                                      $"Посилання на статтю: {article.Link}\n" +
                                      $"Дата публікації статті: {article.DateOfPublication}\n");
                    break;
                case "Publication":
                    var publication = (PublicationMaterial)material;
                    Console.WriteLine($"Книга {publication.Title} | {stageStatus}\n" +
                                      $"За авторством: {publication.Author}\n" +
                                      $"Кількість сторінок: {publication.PageCount}\n");
                    break;
                case "Video":
                    var video = (VideoMaterial)material;
                    Console.WriteLine($"Відео: {video.Title} | {stageStatus}\n" +
                                      $"Тривалість: {video.Duration}\n" +
                                      $"Якість: {video.Quality}p\n");
                    break;
            }

            EditNavigationView();
        }

        /// <summary>
        /// Bottom navigation
        /// </summary>
        public static void EditNavigationView()
        {
            Console.WriteLine("Введіть наступні команди:\n" +
                              $"{Command.NextMaterial} - якщо ви переглянули і пройшли цей матеріал\n" +
                              $"{Command.PreviousMaterial} - повернутися до попереднього матеріалу\n" +
                              $"{Command.BackToCourse} - якщо ви хочете повернутися на сторінку курсу\n");
        }
    }
}
