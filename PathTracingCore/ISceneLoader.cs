using System;
using System.Collections.Generic;
using System.Text;

namespace PathTracingCore
{
    interface ISceneLoader
    {
        List<IHitable> LoadScene(string fileName);
    }
}
