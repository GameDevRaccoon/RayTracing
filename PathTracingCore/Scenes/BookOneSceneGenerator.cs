using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class BookOneSceneGenerator : ISceneGenerator
    {
        private IRandomGenerator randomGenerator;
        public BookOneSceneGenerator(IRandomGenerator randomGenerator)
        {
            this.randomGenerator = randomGenerator;
        }
        public List<IHitable> GenerateScene()
        {
            int n = 500;
            List<IHitable> hitables = new List<IHitable>(n + 1)
            {
                new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f)))
            };
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float chooseMat = randomGenerator.GetRandomFloat();
                    Vector3 center = new Vector3(a + 0.9f * randomGenerator.GetRandomFloat(),
                        0.2f,
                        b + 0.9f * randomGenerator.GetRandomFloat());
                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        if (chooseMat < 0.8f)
                        {
                            hitables.Add(new Sphere(center, 0.2f, new Lambertian(new Vector3(
                                randomGenerator.GetRandomFloat() * randomGenerator.GetRandomFloat(),
                                randomGenerator.GetRandomFloat() * randomGenerator.GetRandomFloat(),
                                randomGenerator.GetRandomFloat() * randomGenerator.GetRandomFloat()))));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            hitables.Add(new Sphere(center, 0.2f,
                                 new Metal(new Vector3(0.5f * (1 + randomGenerator.GetRandomFloat()),
                                 0.5f * (1 + randomGenerator.GetRandomFloat()),
                                 0.5f * randomGenerator.GetRandomFloat()))));
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
    }
}
