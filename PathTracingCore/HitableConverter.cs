using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    class HitableConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IHitable).IsAssignableFrom(objectType) || typeof(Material).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            if (item["Type"].Value<string>() == "Sphere")
            {
                return JsonConvert.DeserializeObject(item.ToString(),typeof(Sphere), new MaterialConverter());
            }
            else if (item["Type"].Value<string>() == "Metal")
            {
                return JsonConvert.DeserializeObject(item.ToString(),typeof(Metal),new MaterialConverter() );
            }
            else if (item["Type"].Value<string>() == "Dielectric")
            {
                return JsonConvert.DeserializeObject(item.ToString(), typeof(Dielectric), new MaterialConverter()); ;
            }
            else if (item["Type"].Value<string>() == "Lambertian")
            {
                return JsonConvert.DeserializeObject(item.ToString(), typeof(Lambertian), new MaterialConverter()); ;
            }
            throw new NotImplementedException("Object did not match any definitions of IHitable");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
