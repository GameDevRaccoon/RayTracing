using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class Dielectric : Material
    {
        public float refIndex;
        public Random rand = new Random();

        public override string Type => "Dielectric";

        public Dielectric(float ri)
        {
            refIndex = ri;
        }

        public float schlick(float cosine, float refI)
        {
            float r0 = (1 - refI) / (1 + refI);
            r0 = r0 * r0;
            return r0 + ((1 - r0) * (float)Math.Pow((1 - cosine), 5));
        }

        public override bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 outwardNormal;
            Vector3 reflected = Reflect(ray.Direction, record.normal);
            float niOverNt;
            attenuation = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 refracted;
            float reflectProbabilty;
            float cosine;
            float dotRayDirectionRecordNormal = Vector3.Dot(ray.Direction, record.normal);
            if ( dotRayDirectionRecordNormal > 0)
            {
                outwardNormal = -record.normal;
                niOverNt = refIndex;
                cosine = refIndex * (dotRayDirectionRecordNormal / ray.Direction.Length());
            }
            else
            {
                outwardNormal = record.normal;
                niOverNt = 1.0f / refIndex;
                cosine = -dotRayDirectionRecordNormal / ray.Direction.Length();
            }

            if (Refract(ray.Direction, outwardNormal, niOverNt, out refracted))
            {
                reflectProbabilty = schlick(cosine, refIndex);
            }
            else
            {
                reflectProbabilty = 1.0f;
            }
            if (Program.RandomGenerator.Value.NextDouble() < reflectProbabilty)
            {
                scattered = new Ray(record.p, reflected);
            }
            else
            {
                scattered = new Ray(record.p, refracted);
            }
            return true;
        }
    }
}
