using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PathTracingCore
{
    class JsonSceneLoader : ISceneLoader
    {
        public List<IHitable> LoadScene(string fileName)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            List<IHitable> hitables;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string reader = streamReader.ReadToEnd();
                hitables = JsonConvert.DeserializeObject <List<IHitable>>(reader, new HitableConverter());
            }
            return hitables;
        }
    }
}
