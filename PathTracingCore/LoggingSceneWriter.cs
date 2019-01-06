using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PathTracingCore
{
    class LoggingSceneWriter : ISceneWriter
    {
        private ISceneWriter SceneWriter { get; }
        public LoggingSceneWriter(ISceneWriter sceneWriter)
        {
            SceneWriter = sceneWriter;
        }
        public void WriteSceneToFile(List<IHitable> hitables)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"SceneWriter\"");
            stopwatch.Start();
            SceneWriter.WriteSceneToFile(hitables);
            stopwatch.Stop();
            Console.WriteLine("Task \"SceneWriter\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
        }
    }
}
