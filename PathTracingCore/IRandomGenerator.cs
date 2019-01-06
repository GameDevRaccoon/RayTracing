using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    interface IRandomGenerator
    {
        float GetRandomFloat();
        float GetRandomFloat(float min, float max);
    }
}
