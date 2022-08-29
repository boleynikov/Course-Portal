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
            return jo["Type"].Value<string>() == "Article"
                ? jo.ToObject<ArticleMaterial>(serializer)
                : jo["Type"].Value<string>() == "Publication"
                ? jo.ToObject<PublicationMaterial>(serializer)
                : jo["Type"].Value<string>() == "Video" ? jo.ToObject<VideoMaterial>(serializer) : null;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
