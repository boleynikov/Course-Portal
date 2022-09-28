using Data.Repository;
using Domain.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;
using Data.Repository.Interface;

namespace Data.Context
{
    public class FileDbContext : IDbContext
    {/// <inheritdoc/>
        public async Task<IEnumerable<T>> Get<T>()
            where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";

            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            var justReadedJson = await File.ReadAllTextAsync(fileName);
            if (string.IsNullOrEmpty(justReadedJson))
            {
                return new List<T>();
            }

            Newtonsoft.Json.JsonConverter[] converters = { new MaterialConverter() };
            var readedList = JsonConvert.DeserializeObject<List<T>>(justReadedJson, new JsonSerializerSettings() { Converters = converters });
            return readedList;
        }

        /// <inheritdoc/>
        public async Task<bool> Update<T>(IEnumerable<T> listEntities)
            where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";
            try
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                };

                var jsonString = JsonConvert.SerializeObject(listEntities, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                });

                await File.WriteAllTextAsync(fileName, jsonString);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
