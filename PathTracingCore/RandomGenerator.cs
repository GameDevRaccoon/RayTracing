using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PathTracingCore
{
    class RandomGenerator : IRandomGenerator
    {
        private static ThreadLocal<Random> randomGenerator = new ThreadLocal<Random>(() => new Random());
        public float GetRandomFloat()
        {
            return (float)randomGenerator.Value.NextDouble();
        }

        public float GetRandomFloat(float min, float max)
        {
            return (float)randomGenerator.Value.NextDouble() * (max - min) + min;
        }
    }
}
