using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    class MaterialConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Material).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            if (item["Type"].Value<string>() == "Metal")
            {
                return item.ToObject<Metal>();
            }
            else if (item["Type"].Value<string>() == "Dielectric")
            {
                return item.ToObject<Dielectric>();
            }
            else if (item["Type"].Value<string>() == "Lambertian")
            {
                return item.ToObject<Lambertian>();
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
