using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    interface ISceneWriter
    {
        void WriteSceneToFile(List<IHitable> hitables);
    }
}
