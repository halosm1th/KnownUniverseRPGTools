// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Xsl;
using TravellerMapSystem.Tools;

namespace KUP_Simple_Sector_Generator;

class Tester
{
    public static void Main()
    {
        var name = "Malands";
        var seed = 01122000;
        var usingSeed = true;
        var printSubImages = true;

        if(usingSeed) Console.WriteLine($"Creating {name} with seed: {seed}");
        else Console.WriteLine($"Creating {name}");
        var tester = new KURpgSubsectorGenerator(name,usingSeed, seed, false);
        
        Console.WriteLine($"Generating {name}");
        tester.Generate();

        Console.WriteLine("Finished Generation");

        Console.WriteLine($"Writing {name} to {Directory.GetCurrentDirectory() + "/" + name}.md");
            tester.WriteToFile(name, Directory.GetCurrentDirectory() + "/Generated Data/");

            Console.WriteLine($"Generating Image for {name}");
            var imageCreator = new DrawSubsector(tester.Subsector);
            var image = imageCreator.GenerateImage(printSubImages, Directory.GetCurrentDirectory() + "/Generated Data/" );

            Console.WriteLine($"Writing image of {name} to {Directory.GetCurrentDirectory() + "/Generated Data/" + name}.png");
            image.SaveAsPng(Directory.GetCurrentDirectory() + "/Generated Data/" + name + ".png");
    }
}