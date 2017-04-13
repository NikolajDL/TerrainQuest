﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrainQuest.Generator.Generators;
using TerrainQuest.Generator.Graph;
using TerrainQuest.Generator.Graph.Blending;

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
            var flatGenerator1 = new FlatGenerator(300, 300, 1d);
            var flatGenerator2 = new FlatGenerator(300, 300, 0.5d);
            var filename = GetFilename("serial.txt");

            var node1 = new GeneratorNode(flatGenerator1);
            var node2 = new GeneratorNode(flatGenerator2);
            var mixNode = new MixNode();
            mixNode.AddDependency(node1, 0.5f);
            mixNode.AddDependency(node2, 0.5f);
            
            using (var writer = new StreamWriter(filename, false))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, mixNode);
            }
            Process.Start(filename);

            using (var reader = new StreamReader(filename))
            {
                var deserializer = new JsonSerializer();
                var node = (MixNode)deserializer.Deserialize(reader, typeof(MixNode));

                node.Execute();
                var bitmap = node.Result.AsBitmap();

                var filename2 = GetFilename("generated.png");
                bitmap.Save(filename2, ImageFormat.Png);
                Process.Start(filename2);
            }

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
