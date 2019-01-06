using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    interface IRayTracer
    {
        Vector3[] GeneratePixels(Camera camera, List<IHitable> hitables);
    }
}
