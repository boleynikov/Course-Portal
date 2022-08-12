// <copyright file="FileDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Data.Repository.Interface;
    using Domain.Abstract;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// FileDBContext implementation of DBContext.
    /// </summary>
    public class FileDbContext : IDbContext
    {
        /// <inheritdoc/>
        public IEnumerable<T> Get<T>()
            where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";
            Console.WriteLine($"Reading {name}s from the file {fileName}...");

            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            var justReadedJson = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(justReadedJson))
            {
                return new List<T>();
            }

            JsonConverter[] converters = { new MaterialConverter() };
            var readedList = JsonConvert.DeserializeObject<List<T>>(justReadedJson, new JsonSerializerSettings() { Converters = converters });
            return readedList;
        }

        /// <inheritdoc/>
        public bool Update<T>(IEnumerable<T> listEntities)
            where T : BaseEntity
        {
            var name = typeof(T).Name;
            var fileName = $"{name}s.data";
            try
            {
                DefaultContractResolver contractResolver = new ()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
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
            catch (JsonReaderException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
