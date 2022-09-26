// <copyright file="MaterialConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Data.Repository
{
    using System;
    using Domain.CourseMaterials;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Converter for deserialization of list with abstract class heirs.
    /// </summary>
    internal class MaterialConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Material);
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["Type"].Value<string>())
            {
                case "Article":
                    return jo.ToObject<ArticleMaterial>(serializer);
                case "Publication":
                    return jo.ToObject<PublicationMaterial>(serializer);
                case "Video":
                    return jo.ToObject<VideoMaterial>(serializer);
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
