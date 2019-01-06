using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PathTracingCore
{
    class LoggingSceneGenerator : ISceneGenerator
    {
        private ISceneGenerator sceneGenerator;
        public LoggingSceneGenerator(ISceneGenerator sceneGenerator)
        {
            this.sceneGenerator = sceneGenerator;
        }
        public List<IHitable> GenerateScene()
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"SceneGenerator\"");
            stopwatch.Start();
            List<IHitable> hitables = sceneGenerator.GenerateScene();
            stopwatch.Stop();
            Console.WriteLine("Task \"SceneGenerator\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return hitables;
        }
    }
}
