using System.Text;

namespace Simple_Subsector_Generator;

class KURPGSectorGenerator
{
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }

    public bool IsPrinting = false;

    public KURPGSector Sector { get; private set; }

    public KURPGSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting = false)
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
        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(Sector.ToString()));
        
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

    public async Task<KURPGSector> Generate()
    {
        if(IsPrinting) Console.WriteLine($"Starting to generate {Name} Sector");
        Sector = new KURPGSector(Name,Seed);
        var tasks = new List<Task<KURpgSubsector>>();
        for (var x = 0; x < Sector.Subsectors.GetLength(0); x++)
        {
            for (var y = 0; y < Sector.Subsectors.GetLength(1); y++)
            {
                tasks.Add( GenerateSectorData(x,y));
            }  
        }

        await Task.WhenAll(tasks);

        var x1 = 0;
        var y1 = 0;
        foreach (var task in tasks)
        {
            Sector.Subsectors[x1, y1] = task.Result;
            if (y1 < Sector.Subsectors.GetLength(0)-1) y1++;
            else
            {
                y1 = 0;
                if (x1 < Sector.Subsectors.GetLength(1)-1) x1++;
            }
        }


        if(IsPrinting) Console.WriteLine($"Finished generating {Name} Sector");

        return Sector;
    }

    private async Task<KURpgSubsector> GenerateSectorData(int x, int y)
    {
                if (IsPrinting) Console.WriteLine($"Begining generation of {Name} {x} {y} Subsector");
                var generator = new KURpgSubsectorGenerator(Name + $" {x},{y}", UsingSeed, Seed + x + y, IsPrinting);
                var result = generator.Generate();

                return await result;
    }
}