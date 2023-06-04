using System.Text;

namespace Simple_Subsector_Generator;

class KURPGSuperSectorGenerator
{
    
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }
    public bool IsPrinting = false;

    public KURPGSuperSector? SuperSector { get; private set; }

    public KURPGSuperSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting)
    {
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        Name = KURpgSubsectorGenerator.GetCountryName(Seed);
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
            //Console.WriteLine($"Writing: ("+ s + ") to file.");
            sw.Flush();
        }
        sw.Close();
        fs.Close();
    }

    
    public async Task<KURPGSuperSector?> Generate()
    {
        if (IsPrinting)
        {
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Starting to generate {Name} SuperSector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        SuperSector = new KURPGSuperSector(Name,Seed);
        
        var tasks = new Task[SuperSector.Sectors.GetLength(0),
            SuperSector.Sectors.GetLength(1)];

        Parallel.For(0, SuperSector.Sectors.GetLength(0), x =>
        {
            Parallel.For(0, SuperSector.Sectors.GetLength(1), y =>
            {
                tasks[x,y] = GenerateSuperSectorData(x, y);
            });
        });

        await Task.WhenAll(tasks.Cast<Task>()); // Wait for all subsector generation tasks to complete

        if (IsPrinting)
        {
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Finished generating {Name} SuperSector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        return SuperSector;
    }
    
    private async Task<KURPGSector?> GenerateSuperSectorData(int x, int y)
    {
        //if(IsPrinting) Console.WriteLine($"Beginning generation of {Name} {x} {y} Sector");
        var generator = new KURPGSectorGenerator(Name + $" {x},{y} Sector", UsingSeed, Seed + x + y, IsPrinting);
        var result = generator.Generate();

        if (SuperSector != null) SuperSector.Sectors[x, y] = await result;

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