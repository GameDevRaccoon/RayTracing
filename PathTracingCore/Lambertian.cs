using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

namespace PathTracingCore
{
    class Lambertian : Material
    {
        Vector3 albedo;

        public Lambertian(in Vector3 a)
        {
            albedo = a;
        }
        public override bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 target = record.p + record.normal + ray.RandomInUnitSphere();
            scattered = new Ray(record.p, target - record.p);
            attenuation = albedo;
            return true;
        }
    }
}
