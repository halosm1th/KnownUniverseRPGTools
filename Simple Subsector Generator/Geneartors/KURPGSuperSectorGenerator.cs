using System.Text;

namespace Simple_Subsector_Generator;

class KURPGSuperSectorGenerator
{
    
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }
    public bool IsPrinting = false;

    public KURPGSuperSector SuperSector { get; private set; }

    public KURPGSuperSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting)
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
        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(SuperSector.ToString()));
        
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

    
    public async void Generate()
    {
        if(IsPrinting) Console.WriteLine($"Starting to generate {Name} SuperSector");
        SuperSector = new KURPGSuperSector(Name,Seed);
        var tasks = new List<Task<KURPGSector>>();
        for (var x = 0; x < SuperSector.Sectors.GetLength(0); x++)
        {
            for (var y = 0; y < SuperSector.Sectors.GetLength(1); y++)
            {
                tasks.Add( GenerateSuperSectorData(x,y));
            }  
        }

        var x1 = 0;
        var y1 = 0;
        foreach (var task in tasks)
        {
            SuperSector.Sectors[x1, y1] = await task;
            if (y1 < SuperSector.Sectors.GetLength(0)-1) y1++;
            else
            {
                y1 = 0;
                if (x1 < SuperSector.Sectors.GetLength(1)-1) x1++;
            }
        }


        if(IsPrinting) Console.WriteLine($"Finished generating {Name} SuperSector");
    }
    
    private async Task<KURPGSector> GenerateSuperSectorData(int x, int y)
    {
        if(IsPrinting) Console.WriteLine($"Beginning generation of {Name} {x} {y} Sector");
        var generator = new KURPGSectorGenerator(Name + $" {x},{y}", UsingSeed, Seed + x + y, IsPrinting);
        var result = generator.Generate();

        return await result;
    }
    
    /*public void Generate()
    {
        if(IsPrinting) Console.WriteLine($"Starting to generate {Name} SuperSector");
        SuperSector = new KURPGSuperSector(Name,Seed);
        for (int x = 0; x < SuperSector.Sectors.GetLength(0); x++)
        {
            for (int y = 0; y < SuperSector.Sectors.GetLength(1); y++)
            {
                if(IsPrinting) Console.WriteLine($"Beginning generation of {Name} {x} {y} Sector");
                var generator = new KURPGSectorGenerator(Name + $" {x},{y}", UsingSeed, Seed + x + y, IsPrinting);
                generator.Generate();
                SuperSector.Sectors[x, y] = generator.Sector;
            }
        }
        if(IsPrinting) Console.WriteLine($"Finished generating {Name} SuperSector");
    }*/
}