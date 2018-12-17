using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Extensions
{
    public static class ExtensionMethods
    {

        public static Vector3 UnitVector(this Vector3 vector3)
        {
            return vector3 / vector3.Length();
        }
    }
}
