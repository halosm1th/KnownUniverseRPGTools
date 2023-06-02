using System.Text;

namespace Simple_Subsector_Generator;

class KURPGMegaSectorGenerator
{
    
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }
    public bool IsPrinting = false;

    public KURPGMegaSector MegaSector { get; private set; }

    public KURPGMegaSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting)
    {
        Name = name;
        Seed = seed;
        UsingSeed = usingSeed;
        IsPrinting = isPrinting;
    }

    
    
    private static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
        "UTF-8",
        new EncoderReplacementFallback(string.Empty),
        new DecoderExceptionFallback()
    );
    
    public void WriteToFile(string name, string path)
    {
        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(MegaSector.ToString()));
        
        var st = utf8Text;
        
        var fs = File.OpenWrite(path + $"/{name}.md");
        var sw = new StreamWriter(fs);
        foreach (var s in st.Split('\n'))
        {
            sw.WriteLine(s);
            Console.WriteLine($"Writing: ("+ s + ") to file.");
            sw.Flush();
        }
        sw.Close();
        fs.Close();
    }

    public void Generate()
    {
        if(IsPrinting) Console.WriteLine($"Starting to generate {Name} Megasector");
        MegaSector = new KURPGMegaSector(Name,Seed);
        for (int x = 0; x < MegaSector.SuperSectors.GetLength(0); x++)
        {
            for (int y = 0; y < MegaSector.SuperSectors.GetLength(1); y++)
            {
                if(IsPrinting) Console.WriteLine($"Begining to generate {Name} {x} {y}");
                var generator = new KURPGSuperSectorGenerator(Name + $" {x},{y}", UsingSeed, Seed + x + y, IsPrinting);
                generator.Generate();
                MegaSector.SuperSectors[x, y] = generator.SuperSector;
            }
        }
        if(IsPrinting) Console.WriteLine($"Finished generating {Name} SuperSector");
    }
}