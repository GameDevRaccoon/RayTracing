using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    class Camera
    {
        Vector3 origin;
        Vector3 lowerLeftCorner;
        Vector3 horizontal;
        Vector3 vertical;

        public Camera()
        {
            lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            vertical = new Vector3(0.0f, 2.0f, 0.0f);
            origin = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public Ray GetRay(float u, float v)
        {
            return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
        }
    }
}
