// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Xsl;
using TravellerMapSystem.Tools;

namespace Simple_Subsector_Generator;

class Tester
{
    public static void Main()
    {
        var name = "test of coloured subsectors";
        var seed = 01122000;
        var usingSeed = false;

        if(usingSeed) Console.WriteLine($"Creating {name} with seed: {seed}");
        else Console.WriteLine($"Creating {name}");
        var tester = new KURPGSuperSectorGenerator(name,usingSeed, seed, true);

        Console.WriteLine($"Generating {name}");
        var task = Task.Run(() => tester.Generate());
        while (!task.IsCompleted)
        {
        }

        Console.WriteLine("Finished Generation");

        Console.WriteLine($"Writing {name} to {Directory.GetCurrentDirectory() + "/" + name}.md");
            tester.WriteToFile(name, Directory.GetCurrentDirectory() + "/Generated Data/");

            Console.WriteLine($"Generating Image for {name}");
            var imageCreator = new DrawSuperSector(tester.SuperSector);
            var image = imageCreator.GenerateImage();

            Console.WriteLine($"Writing image of {name} to {Directory.GetCurrentDirectory() + "/Generated Data/" + name}.png");
            image.SaveAsPng(Directory.GetCurrentDirectory() + "/Generated Data/" + name + ".png");
    }
}