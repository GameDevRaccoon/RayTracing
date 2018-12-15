using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class Dielectric : Material
    {
        float refIndex;
        public Dielectric(float ri)
        {
            refIndex = ri;
        }

        public override bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 outwardNormal;
            Vector3 reflected = Reflect(ray.Direction, record.normal);
            float niOverNt;
            attenuation = new Vector3(1.0f, 1.0f, 0.0f);
            Vector3 refracted;
            if (Vector3.Dot(ray.Direction, record.normal) > 0)
            {
                outwardNormal = -record.normal;
                niOverNt = refIndex;
            }
            else
            {
                outwardNormal = record.normal;
                niOverNt = 1.0f / refIndex;
            }

            if (Refract(ray.Direction, outwardNormal, niOverNt, out refracted))
            {
                scattered = new Ray(record.p, refracted);
            }
            else
            {
                scattered = new Ray(record.p, reflected);
                return false;
            }
            return true;
        }
    }
}
