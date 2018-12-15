using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class Sphere : IHitable
    {
        public Sphere() { }
        public Sphere(Vector3 center, float r, Material material)
        {
            this.center = center;
            radius = r;
            this.material = material;
        }
        public bool Hit(Ray r, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();
            Vector3 oc = r.Origin - center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float b = Vector3.Dot(oc, r.Direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = b * b - a * c;
            if (discriminant > 0)
            {
                float temp = (-b - (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    record.t = temp;
                    record.p = r.PointAt(record.t);
                    record.normal = (record.p - center) / radius;
                    record.material = material;
                    return true;
                }
                temp = (-b + (float)Math.Sqrt(b * b - a * c)) / a;
                if (temp <tMax && temp > tMin)
                {
                    record.t = temp;
                    record.p = r.PointAt(record.t);
                    record.normal = (record.p - center) / radius;
                    record.material = material;
                    return true;
                }
            }
            return false;
        }

        Vector3 center;
        float radius;
        Material material;
    }
}
