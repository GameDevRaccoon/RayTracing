using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    interface ISceneGenerator
    {
        List<IHitable> GenerateScene();
    }
}
