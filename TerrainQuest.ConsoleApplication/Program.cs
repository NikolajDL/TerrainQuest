using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TerrainQuest.Generator.Blending;
using TerrainQuest.Generator.Effects;
using TerrainQuest.Generator.Generators;
using TerrainQuest.Generator.Generators.Noise;
using TerrainQuest.Generator.Generators.Shape;
using TerrainQuest.Generator.Graph;
using TerrainQuest.Generator.Graph.Blending;
using TerrainQuest.Generator.Graph.Effect;

namespace TerrainQuest.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            SerializeFlatGenerator();
        }

        private static void SerializeFlatGenerator()
        {
            //var generator1 = new SquareGenerator(200, 200, new Size(150, 150), 0.5f);
            //var generator2 = new SquareGenerator(200, 200, new Size(150, 150), new Point(50,50), 0.5f);
            var generator1 = new FlatGenerator(200, 200, 1);
            var generator2 = new DSNoiseGenerator(200, 200, seed: 10);
            var filename = GetFilename("serial.json");

            var node1 = new GeneratorNode(generator1);
            var node2 = new GeneratorNode(generator2);

            var blend = new BlendingNode(BlendModes.Difference);
            blend.AddDependency(node1);
            blend.AddDependency(node2);


            blend.Execute();
            var bitmap = blend.Result.AsBitmap();
            var filename2 = GetFilename("generated.png");
            bitmap.Save(filename2, ImageFormat.Png);

            Process.Start(filename2);
        }

        private static void GenerateFlatTestMap()
        {
            var flatGenerator = new FlatGenerator(300, 300, 0.5d);
            var node = new GeneratorNode(flatGenerator);

            node.Execute();
            var bitmap = node.Result.AsBitmap();

            var filename = GetFilename("generated.png");
            bitmap.Save(filename, ImageFormat.Png);
            Process.Start(filename);
        }

        private static string GetFilename(string filename)
        {
            return Path.Combine(Environment.CurrentDirectory, filename);
        }
    }
}
