using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class LoggingRayTracer : IRayTracer
    {
        private IRayTracer rayTracer;
        public LoggingRayTracer(IRayTracer rayTracer)
        {
            this.rayTracer = rayTracer;
        }
        public Vector3[] GeneratePixels(Camera camera, List<IHitable> hitables)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"RayTracer\"");
            stopwatch.Start();
            Vector3[] ret = rayTracer.GeneratePixels(camera, hitables);
            stopwatch.Stop();
            Console.WriteLine("Task \"RayTracer\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return ret;
        }
    }
}
