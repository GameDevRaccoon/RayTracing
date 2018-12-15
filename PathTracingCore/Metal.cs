using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class Metal : Material
    {
        Vector3 albedo;
        float fuzz;
        public Metal(in Vector3 a, float f)
        {
            albedo = a;
            fuzz = f < 1 ? f : 1;
        }
        public override bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 reflected = Reflect((ray.Direction / ray.Direction.Length()), record.normal);
            scattered = new Ray(record.p, reflected + fuzz*ray.RandomInUnitSphere());
            attenuation = albedo;
            return (Vector3.Dot(scattered.Direction, record.normal) > 0);
        }
    }
}
