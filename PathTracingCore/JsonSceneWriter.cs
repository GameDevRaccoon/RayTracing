using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PathTracingCore
{
    class JsonSceneWriter : ISceneWriter
    {
        private string FileName { get; }
        public JsonSceneWriter(string fileName)
        {
            FileName = fileName;
        }
        public void WriteSceneToFile(List<IHitable> hitables)
        {
            var outputFile = System.IO.File.Create(FileName);
            using (StreamWriter streamWriter = new StreamWriter(outputFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(streamWriter, hitables.ToArray());
            }
        }
    }
}
