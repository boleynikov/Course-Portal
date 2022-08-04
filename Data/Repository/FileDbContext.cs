using Data.Repository.Abstract;
using Domain.Abstract;
using Domain.CourseMaterials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Data.Repository
{
    public class FileDbContext : IDbContext
    {
        public List<T> Get<T>() where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";
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

            JsonConverter[] converters = { new MaterialConverter() };
            var readedList = JsonConvert.DeserializeObject<List<T>>(justReadedJson, new JsonSerializerSettings() { Converters = converters });
            return readedList;
        }

        public bool Update<T>(List<T> listEntities) where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";
            try
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                var jsonString = JsonConvert.SerializeObject(listEntities, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                });

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

    internal class MaterialConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Material));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["type"].Value<string>() == "Article")
                return jo.ToObject<ArticleMaterial>(serializer);

            if (jo["type"].Value<string>() == "Publication")
                return jo.ToObject<PublicationMaterial>(serializer);

            if (jo["type"].Value<string>() == "Video")
                return jo.ToObject<VideoMaterial>(serializer);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
