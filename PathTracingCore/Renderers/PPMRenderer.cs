using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class PPMRenderer : IRenderer
    {
        private string OutputFileName { get; }
        private Vector2 ImageSize { get; }
        public PPMRenderer(Vector2 imageSize, string outputFileName)
        {
            ImageSize = imageSize;
            OutputFileName = outputFileName;
        }


        public void Render(Vector3[] pixels)
        {
            // Set up an output file
            var outputFile = System.IO.File.Create(OutputFileName);
            using (var fileWriter = new System.IO.StreamWriter(outputFile))
            {
                // Writer continuation
                fileWriter.WriteLine($"P3\n {ImageSize.X} {ImageSize.Y}\n255");

                foreach (var pixel in pixels)
                {

                    int ir = (int)(255.99 * pixel.X);
                    int ig = (int)(255.99 * pixel.Y);
                    int ib = (int)(255.99 * pixel.Z);
                    fileWriter.WriteLine($"{ir} {ig} {ib}");
                }
            }
        }
    }
}
