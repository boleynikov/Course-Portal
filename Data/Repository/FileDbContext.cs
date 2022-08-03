using Data.Repository.Abstract;
using Domain;
using Domain.Abstract;
using Domain.CourseMaterials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class FileDbContext : IDbContext
    {
        public List<T> Get<T>() where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.txt";
            Console.WriteLine($"Reading {name}s from the file {fileName}...");
            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            var justReadedJson = File.ReadAllText(fileName);
            if (String.IsNullOrEmpty(justReadedJson))
            {
                return new List<T>();
            }
            var readedList = JsonConvert.DeserializeObject<List<T>>(justReadedJson);
            return readedList;
        }

        public bool Update<T>(List<T> listEntities) where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.txt";
            try
            {
                var jsonString = JsonConvert.SerializeObject(listEntities);
                File.WriteAllText(fileName, jsonString);
                Console.WriteLine("Дані оновлено.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
