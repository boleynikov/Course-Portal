// <copyright file="MaterialConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    using System;
    using Domain.CourseMaterials;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Converter for deserialization of list with abstract class heirs.
    /// </summary>
    public class MaterialConverter : JsonConverter
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
            //var jo = JObject.Load(reader);
            //return jo["Type"].Value<string>() switch
            //{
            //    "Article" => jo.ToObject<ArticleMaterial>(serializer),
            //    "Publication" => jo.ToObject<PublicationMaterial>(serializer),
            //    "Video" => jo.ToObject<VideoMaterial>(serializer),
            //};
            JObject jo = JObject.Load(reader);
            if (jo["type"].Value<string>() == "Article")
                return jo.ToObject<ArticleMaterial>(serializer);

            if (jo["type"].Value<string>() == "Publication")
                return jo.ToObject<PublicationMaterial>(serializer);

            if (jo["type"].Value<string>() == "Video")
                return jo.ToObject<VideoMaterial>(serializer);

            return null;

        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.FromObject(value);
            if (token.Type != JTokenType.Object)
            {
                token.WriteTo(writer);
            }
            else
            {
                var jo = (JObject)token;
                IList<string> propertyNames = jo.Properties().Select(p => p.Name).ToList();
                jo.AddFirst(new JProperty("Properties", new JArray(propertyNames)));
                jo.WriteTo(writer);
            }
        }
    }
}
