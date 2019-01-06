using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    struct HitRecord
    {
        public float t;
        public Vector3 p;
        public Vector3 normal;
        public Material material;
    }

    interface IHitable
    {
        string Type { get; }
        bool Hit(Ray r, float tMin, float tMax, out HitRecord record);
    }
}
