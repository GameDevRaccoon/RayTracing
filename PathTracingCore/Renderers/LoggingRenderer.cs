using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class LoggingRenderer : IRenderer
    {
        private IRenderer renderer;
        public LoggingRenderer(IRenderer renderer)
        {
            this.renderer = renderer;
        }
        public void Render(Vector3[] pixels)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"Renderer\"");
            stopwatch.Start();
            renderer.Render(pixels);
            stopwatch.Stop();
            Console.WriteLine("Task \"Renderer\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
        }
    }
}
