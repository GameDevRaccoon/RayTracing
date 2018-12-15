using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PathTracingCore
{

    class Program
    {
        private string OutputFileName { get; } = "C:\\Users\\Konna\\source\\repos\\RayTracing\\output.ppm";
        public Vector2 ImageSize { get; set; } = new Vector2(1920, 1080);
        public int NumberOfSamples { get; } = 100;
        public static ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random());

        public static void Main(string[] args) { new Program().MainAsync().GetAwaiter().GetResult(); }

        public async Task MainAsync()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine($"Rendering image of {ImageSize.X}x{ImageSize.Y}!");
            Console.WriteLine($"Generation of {ImageSize.X * ImageSize.Y} pixels...");
            Camera camera = new Camera();
            List<IHitable> hitables = new List<IHitable>
            {
                new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new Vector3(0.8f,0.3f,0.3f))),
                new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))),
                new Sphere(new Vector3(1,0,-1f), 0.5f, new Metal(new Vector3(0.8f,0.6f,0.2f),0.3f)),
                new Sphere(new Vector3(-1,0,-1), 0.5f, new Dielectric(1.5f))
            };

            Vector3[] pixels = await MeasureVector3TaskTimeAsync(GeneratePixelsAsync, camera, hitables, "Generator");
            await MeasureTaskTimeAsync(RenderToPPM, pixels, "Render");

            Console.WriteLine("Render Complete!");
            Console.WriteLine("-----------------------------------------------");
            Console.ReadKey();
        }

        public async Task MeasureTaskTimeAsync(Func<Vector3[], Task> action, Vector3[] arg1, string taskName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await action(arg1);
            stopwatch.Stop();
            Console.WriteLine("Task \"" + taskName + "\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");

        }
        public async Task<Vector3[]> MeasureVector3TaskTimeAsync(Func<Camera, List<IHitable>, Task<Vector3[]>> action, Camera camera, List<IHitable> hitables, string taskName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Vector3[] pixels = await action(camera, hitables);
            stopwatch.Stop();
            Console.WriteLine("Task \"" + taskName + "\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return pixels;
        }

        private async Task<Vector3[]> GeneratePixelsAsync(Camera camera, List<IHitable> hitables)
        {
            Vector3[] pixels = new Vector3[(int)ImageSize.X * (int)ImageSize.Y];
            int index = 0;
            for (int j = (int)ImageSize.Y - 1; j >= 0; j--)
            {
                for (int i = 0; i < (int)ImageSize.X; i++)
                {
                    // Anti-Aliasing per pixel
                    // Task for each colour
                    Vector3 col = new Vector3(0, 0, 0);
                    for (int s = 0; s < NumberOfSamples; s++)
                    {
                        float u = (i + (float)RandomGenerator.Value.NextDouble()) / ImageSize.X;
                        float v = (j + (float)RandomGenerator.Value.NextDouble()) / ImageSize.Y;
                        var r = camera.GetRay(u, v);

                        Vector3 p = r.PointAt(2.0f);
                        col += Colour(r, hitables, 0);
                    }

                    // Merge Anti-Aliasing
                    col /= NumberOfSamples;
                    col = new Vector3((float)Math.Sqrt(col.X), (float)Math.Sqrt(col.Y), (float)Math.Sqrt(col.Z));
                    pixels[index++] = col;
                }
            }
            return pixels;
        }

        private async Task RenderToPPM(Vector3[] pixels)
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
            await Task.CompletedTask;
        }

        private Vector3 Colour(in Ray r, List<IHitable> world, int depth)
        {
            HitRecord record = new HitRecord();
            if (HitFromList(r, 0.001f, float.MaxValue, world, out record))
            {
                Ray scattered;
                Vector3 attenuation;
                if (depth < 50 && record.material.Scatter(r, record, out attenuation, out scattered))
                {
                    return attenuation * Colour(scattered, world, depth + 1);
                }
                else
                {
                    return Vector3.Zero;
                }
            }
            else
            {
                var unitDirection = r.Direction / r.Direction.Length();
                float t = 0.5f * (unitDirection.Y + 1.0f);
                return (1.0f - t) * Vector3.One + (t * new Vector3(0.5f, 0.7f, 1.0f));
            }
        }

        private bool HitFromList(in Ray ray, float tMin, float tMax, List<IHitable> hitables, out HitRecord record)
        {
            HitRecord tempRecord;
            record = new HitRecord();
            bool hitAnything = false;
            float closestSoFar = tMax;
            for (int i = 0; i < hitables.Count; i++)
            {
                if (hitables[i].Hit(ray, tMin, closestSoFar, out tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.t;
                    record = tempRecord;
                }
            }
            return hitAnything;
        }
    }
}
