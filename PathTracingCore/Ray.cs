using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

namespace PathTracingCore
{
    class Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }
        public static ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random());

        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 PointAt(float t)
        {
            return Origin + (t * Direction);
        }

        private float RandomFloat(float minimum, float maximum)
        {
            return (float)RandomGenerator.Value.NextDouble() * (maximum - minimum) + minimum;
        }

        public Vector3 RandomInUnitSphere()
        {
            Vector3 p = new Vector3();
            do
            {
                p = 2.0f * new Vector3(RandomFloat(-1.0f, 1.0f), RandomFloat(-1.0f, 1.0f), RandomFloat(-1.0f, 1.0f)) - Vector3.One;
            } while (p.LengthSquared() >= 1.0);
            return p;
        }
    }
}
