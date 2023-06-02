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
        var name = "Barbetii";
        var seed = 01122000;
        var usingSeed = true;

        if(usingSeed) Console.WriteLine($"Creating {name} with seed: {seed}");
        else Console.WriteLine($"Creating {name}");
        var tester = new KURPGMegaSectorGenerator(name,usingSeed, seed, true);
        
        Console.WriteLine($"Generating {name}");
        tester.Generate();
        Console.WriteLine($"Writing {name} to {Directory.GetCurrentDirectory()+"/"+ name}.md");
        tester.WriteToFile(name, Directory.GetCurrentDirectory()+"/Generated Data/");
        
        Console.WriteLine($"Generating Image for {name}");
        var imageCreator = new DrawMegaSector(tester.MegaSector);
        var image = imageCreator.GenerateSectorImage();
        
        Console.WriteLine($"Writing image of {name} to {Directory.GetCurrentDirectory()+"/"+ name}.png");
        image.SaveAsPng(Directory.GetCurrentDirectory()+"/"+name+".png");
    }
}