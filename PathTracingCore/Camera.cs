using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Extensions;
namespace PathTracingCore
{
    class Camera
    {
        Vector3 origin;
        Vector3 lowerLeftCorner;
        Vector3 horizontal;
        Vector3 vertical;
        Vector3 u, v, w;
        float lensRadius;

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vup, float vfov, float aspect, float aperture, float focusDistance)
        {
            lensRadius = aperture / 2;
            float theta = vfov * MathF.PI / 180;
            float halfHeight = MathF.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;
            origin = lookFrom;
            w = (lookFrom - lookAt).UnitVector();
            u = (Vector3.Cross(vup, w)).UnitVector();
            v = Vector3.Cross(w, u);
            lowerLeftCorner = origin - halfWidth*focusDistance * u - halfHeight* focusDistance * v - focusDistance* w;
            horizontal = 2 * halfWidth*focusDistance * u;
            vertical = 2 * halfHeight*focusDistance * v;
        }

        private float RandomFloat(float minimum, float maximum)
        {
            return (float)Program.RandomGenerator.Value.NextDouble() * (maximum - minimum) + minimum;
        }

        public Vector3 RandomInUnitDisk()
        {
            Vector3 p = new Vector3();
            do
            {
                p = 2.0f * new Vector3(RandomFloat(-1.0f, 1.0f), RandomFloat(-1.0f, 1.0f),0) - new Vector3(1,1,0);
            } while (Vector3.Dot(p,p) >= 1.0);
            return p;
        }

        public Ray GetRay(float s, float t)
        {
            Vector3 rd = lensRadius * RandomInUnitDisk();
            Vector3 offset = u * rd.X + v * rd.Y;
            return new Ray(origin +offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset);
        }
    }
}
