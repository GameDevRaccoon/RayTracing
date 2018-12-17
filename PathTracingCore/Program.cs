using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Extensions;

namespace PathTracingCore
{

    class Program
    {
        private string OutputFileName { get; } = "C:\\Users\\Conor Wood\\com\\konna\\RayTracing\\output.ppm";
        public Vector2 ImageSize { get; set; } = new Vector2(1920, 1080);
        public int NumberOfSamples { get; } = 100;
        public static ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random());

        public static void Main(string[] args) { new Program().MainAsync().GetAwaiter().GetResult(); }
        public float PixelCount { get { return (ImageSize.X * ImageSize.Y); } }

        public async Task MainAsync()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine($"Rendering image of {ImageSize.X}x{ImageSize.Y}!");
            Console.WriteLine($"Generation of {PixelCount} pixels...");
            Vector3 lookFrom = new Vector3(13, 2, 3);
            Vector3 lookAt = new Vector3(0, 0, 0);
            float distanceToFocus = 10;
            float aperture = 0.1f;
            Camera camera = new Camera(lookFrom, lookAt, new Vector3(0,1,0),20, ImageSize.X / ImageSize.Y,aperture,distanceToFocus);
            List<IHitable> hitables = await MeasureListHitablesTaskTimeAsync(RandomScene, "Scene Generation");

            Vector3[] pixels = await MeasureVector3TaskTimeAsync(GeneratePixelsAsync, camera, hitables, "Generator");
            await MeasureTaskTimeAsync(RenderToPPM, pixels, "Render");

            Console.WriteLine("Render Complete!");
            Console.WriteLine("-----------------------------------------------");
            Console.ReadKey();
        }

        private async Task<List<IHitable>> MeasureListHitablesTaskTimeAsync(Func<Task<List<IHitable>>> action, string taskName)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"{taskName}\"");
            stopwatch.Start();
            List<IHitable> hitables = await action();
            stopwatch.Stop();
            Console.WriteLine("Task \"" + taskName + "\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return hitables;
        }

        public async Task<List<IHitable>> RandomScene()
        {
            int n = 500;
            List<IHitable> hitables = new List<IHitable>(n + 1)
            {
                new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f)))
            };
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11;b < 11; b++)
                {
                    float chooseMat = (float)RandomGenerator.Value.NextDouble();
                    Vector3 center = new Vector3(a + 0.9f * (float)RandomGenerator.Value.NextDouble(),
                        0.2f,
                        b + 0.9f * (float)RandomGenerator.Value.NextDouble());
                    if ((center - new Vector3(4,0.2f,0)).Length() > 0.9f)
                    {
                        if (chooseMat <0.8f)
                        {
                            hitables.Add(new Sphere(center, 0.2f, new Lambertian(new Vector3(
                                (float)RandomGenerator.Value.NextDouble() * (float)RandomGenerator.Value.NextDouble(),
                                (float)RandomGenerator.Value.NextDouble() * (float)RandomGenerator.Value.NextDouble(),
                                (float)RandomGenerator.Value.NextDouble() * (float)RandomGenerator.Value.NextDouble()))));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            hitables.Add(new Sphere(center, 0.2f,
                                 new Metal(new Vector3(0.5f * (1 + (float)RandomGenerator.Value.NextDouble()),
                                 0.5f * (1 + (float)RandomGenerator.Value.NextDouble()),
                                 0.5f * (float)RandomGenerator.Value.NextDouble()))));
                        }
                        else
                        {
                            hitables.Add(new Sphere(center, 0.2f, new Dielectric(1.5f)));
                        }
                    }
                }
            }
            hitables.Add(new Sphere(new Vector3(0, 1, 0), 1.0f, new Dielectric(1.5f)));
            hitables.Add(new Sphere(new Vector3(-4, 1, 0), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))));
            hitables.Add(new Sphere(new Vector3(4, 1, 0), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f)));
            return hitables;
        }

        public async Task MeasureTaskTimeAsync(Func<Vector3[], Task> action, Vector3[] arg1, string taskName)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"{taskName}\"");
            stopwatch.Start();
            await action(arg1);
            stopwatch.Stop();
            Console.WriteLine("Task \"" + taskName + "\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");

        }
        public async Task<Vector3[]> MeasureVector3TaskTimeAsync(Func<Camera, List<IHitable>, Task<Vector3[]>> action, Camera camera, List<IHitable> hitables, string taskName)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Starting Task: \"{taskName}\"");
            stopwatch.Start();
            Vector3[] pixels = await action(camera, hitables);
            stopwatch.Stop();
            Console.WriteLine("Task \"" + taskName + "\" took " + stopwatch.ElapsedMilliseconds + "ms to run.");
            return pixels;
        }

        private async Task<Vector3[]> GeneratePixelsAsync(Camera camera, List<IHitable> hitables)
        {
            Vector3[] pixels = new Vector3[(int)PixelCount];
            int index = 0;
            for (int j = (int)ImageSize.Y - 1; j >= 0; j--)
            {
                for (int i = 0; i < (int)ImageSize.X; i++)
                {
                    // Anti-Aliasing per pixel
                    // Task for each colour
                    Vector3 col = new Vector3(0, 0, 0);
                    Vector3[] colours = new Vector3[100];
                    Parallel.For(0, NumberOfSamples, s =>
                    {
                        float u = (i + (float)RandomGenerator.Value.NextDouble()) / ImageSize.X;
                        float v = (j + (float)RandomGenerator.Value.NextDouble()) / ImageSize.Y;
                        var r = camera.GetRay(u, v);

                        Vector3 p = r.PointAt(2.0f);
                        colours[s] = Colour(r, hitables, 0);
                    });
                    for (int l = 0; l < 100; l++)
                    {
                        col += colours[l];
                    }
                    // Merge Anti-Aliasing
                    col /= NumberOfSamples;
                    col = new Vector3((float)Math.Sqrt(col.X), (float)Math.Sqrt(col.Y), (float)Math.Sqrt(col.Z));
                    Console.Write($"\rGenerated pixel: {index} of {PixelCount} {(float)index / PixelCount *  100f}% Complete.   ");
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
                var unitDirection = r.Direction.UnitVector();
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
