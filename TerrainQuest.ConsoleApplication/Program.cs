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

namespace TerrainQuest.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var flatGenerator = new FlatGenerator(300, 300, 0.5d);
            var node = new GeneratorNode(flatGenerator);

            node.Execute();
            var bitmap = node.Result.AsBitmap();

            var filename = Path.Combine(Environment.CurrentDirectory, "generated.png");
            bitmap.Save(filename, ImageFormat.Png);
            Process.Start(filename);
            Console.ReadKey();
        }
    }
}
