using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Extensions;

namespace PathTracingCore
{

    class Program
    {
        private string OutputFileName { get; } = "C:\\Users\\Conor Wood\\com\\konna\\RayTracing\\output.ppm";
        private string JsonOutput { get; } = "C:\\Users\\Conor Wood\\com\\konna\\RayTracing\\scene.json";
        public Vector2 ImageSize { get; set; } = new Vector2(200, 100);
        public int NumberOfSamples { get; } = 100;
        public static ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random());

        public static void Main(string[] args) { new Program().MainAsync().GetAwaiter().GetResult(); }
        public float PixelCount { get { return (ImageSize.X * ImageSize.Y); } }

        public async Task MainAsync()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine($"Rendering image of {ImageSize.X}x{ImageSize.Y}!");
            Console.WriteLine($"Generation of {PixelCount} pixels...");
            Vector3 lookFrom = new Vector3(13, 2, 3);
            Vector3 lookAt = new Vector3(0, 0, 0);
            float distanceToFocus = 10;
            float aperture = 0.1f;
            Vector3 vup = new Vector3(0, 1, 0);

            // Random Generator
            IRandomGenerator randomGenerator = new RandomGenerator();

            Camera camera = new Camera(lookFrom, lookAt, vup,20, ImageSize.X / ImageSize.Y,aperture,distanceToFocus);

            // Scene Generator 
            ISceneGenerator sceneGenerator = new BookOneSceneGenerator(randomGenerator);
            LoggingSceneGenerator loggingSceneGenerator = new LoggingSceneGenerator(sceneGenerator);
            List<IHitable> hitables = loggingSceneGenerator.GenerateScene();

            // Scene Writer 
            ISceneWriter sceneWriter = new JsonSceneWriter(JsonOutput);
            LoggingSceneWriter loggingSceneWriter = new LoggingSceneWriter(sceneWriter);
            loggingSceneWriter.WriteSceneToFile(hitables);

            // Scene Loader
            ISceneLoader sceneLoader = new JsonSceneLoader();
            LoggingSceneLoader loggingSceneLoader = new LoggingSceneLoader(sceneLoader);
            hitables = loggingSceneLoader.LoadScene(JsonOutput);

            // Ray Tracer
            IRayTracer rayTracer = new PathTracer(ImageSize, NumberOfSamples, randomGenerator);
            LoggingRayTracer loggingRayTracer = new LoggingRayTracer(rayTracer);
            Vector3[] pixels = loggingRayTracer.GeneratePixels(camera, hitables);

            // Renderer
            IRenderer renderer = new PPMRenderer(ImageSize, OutputFileName);
            LoggingRenderer loggingRenderer= new LoggingRenderer(renderer);
            loggingRenderer.Render(pixels);

            Console.WriteLine("Render Complete!");
            Console.WriteLine("-----------------------------------------------");
            Console.ReadKey();
        }
    }
}
