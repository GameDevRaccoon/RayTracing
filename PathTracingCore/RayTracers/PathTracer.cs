using Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PathTracingCore
{
    class PathTracer : IRayTracer
    {
        private int PixelCount { get; }
        private Vector2 ImageSize { get; }
        private int NumberOfSamples { get; }
        private IRandomGenerator RandomGenerator { get; }

        public PathTracer(Vector2 imageSize, int numberOfSamples, IRandomGenerator randomGenerator)
        {

            ImageSize = imageSize;
            PixelCount = (int)(ImageSize.X * ImageSize.Y);
            NumberOfSamples = numberOfSamples;
            RandomGenerator = randomGenerator;
        }

        public Vector3[] GeneratePixels(Camera camera, List<IHitable> hitables)
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
                        float u = (i + RandomGenerator.GetRandomFloat()) / ImageSize.X;
                        float v = (j + RandomGenerator.GetRandomFloat()) / ImageSize.Y;
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
                    col = new Vector3(MathF.Sqrt(col.X), MathF.Sqrt(col.Y), MathF.Sqrt(col.Z));
                    Console.Write($"\rGenerated pixel: {index} of {PixelCount} {(float)index / PixelCount * 100f}% Complete.   ");
                    pixels[index++] = col;
                }
            }
            return pixels;
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
