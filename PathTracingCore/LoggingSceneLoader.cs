using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PathTracingCore
{
    class LoggingSceneLoader : ISceneLoader
    {
        ISceneLoader SceneLoader { get; }
        public LoggingSceneLoader(ISceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }
        public List<IHitable> LoadScene(string fileName)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"SceneLoader\"");
            stopwatch.Start();
            List<IHitable> hitables = SceneLoader.LoadScene(fileName);
            stopwatch.Stop();
            Console.WriteLine("Task \"SceneLoader\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return hitables;
        }
    }
}
