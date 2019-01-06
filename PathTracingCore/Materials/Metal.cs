using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Extensions;

namespace PathTracingCore
{
    class Metal : Material
    {
        public Vector3 albedo;
        public float fuzz;
        public Metal(in Vector3 a, float f = 1f)
        {
            albedo = a;
            fuzz = f < 1 ? f : 1;
        }

        public override string Type => "Metal";

        public override bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 reflected = Reflect(ray.Direction.UnitVector(), record.normal);
            scattered = new Ray(record.p, reflected + fuzz*ray.RandomInUnitSphere());
            attenuation = albedo;
            return (Vector3.Dot(scattered.Direction, record.normal) > 0);
        }
    }
}
