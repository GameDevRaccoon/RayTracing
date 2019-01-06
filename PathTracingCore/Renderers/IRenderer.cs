using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PathTracingCore
{
    interface IRenderer
    {
        void Render(Vector3[] pixels);
    }
}
