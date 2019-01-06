using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Extensions;

namespace PathTracingCore
{
    abstract class Material
    {
        public abstract bool Scatter(in Ray ray, in HitRecord record, out Vector3 attenuation, out Ray scattered);

        public abstract string Type { get; }

        protected Vector3 Reflect(in Vector3 v, in Vector3 n)
        {
            return v - (2 * Vector3.Dot(v, n) * n);
        }

        protected bool Refract(in Vector3 v, in Vector3 n, float niOverNt, out Vector3 refacted)
        {
            refacted = new Vector3();
            Vector3 uv = v.UnitVector();
            float dt = Vector3.Dot(uv, n);
            float discriminant = 1.0f - niOverNt * niOverNt *(1 - dt * dt);
            if (discriminant > 0)
            {
                refacted = niOverNt * (uv - (n * dt)) - n * (float)Math.Sqrt(discriminant);
                return true;
            }
            return false;
        }

    }
}
