using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            var flatGenerator1 = new VoronoiNoiseGenerator(200, 200);
            var filename = GetFilename("serial.json");

            var node1 = new GeneratorNode(flatGenerator1);

            var addNode = new AddNode();
            addNode.AddDependency(node1, 1f);

            var normalize = new NormalizeNode(node1);

            node1.Execute();
            var bitmap = node1.Result.AsBitmap();
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
