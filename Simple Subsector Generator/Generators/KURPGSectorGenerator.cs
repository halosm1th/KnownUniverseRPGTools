using System.Text;

namespace Simple_Subsector_Generator;

class KURPGSectorGenerator
{
    public string Name { get; }
    public int Seed { get; }
    public bool UsingSeed { get; }
    public bool IsPrinting = false;
    public KURPGSector? Sector { get; private set; }

    public KURPGSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting = false)
    {
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        Name = KURPGSubsectorGenerator.GetProvinceName(Seed);
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
        var utf8Text = GeneratorUtils.Utf8Encoder.GetString(
            GeneratorUtils.Utf8Encoder.GetBytes(Sector.ToString()));
        
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

    public async Task<KURPGSector?> Generate()
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Starting to generate {Name} Sector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        Sector = new KURPGSector(Name, Seed);

        var tasks = new Task[Sector.Subsectors.GetLength(0),
            Sector.Subsectors.GetLength(1)];

        Parallel.For(0, Sector.Subsectors.GetLength(0), x =>
        {
            Parallel.For(0, Sector.Subsectors.GetLength(1), y =>
            {
                tasks[x, y] = GenerateSectorData(x, y);
            });
        });
            

        await Task.WhenAll(tasks.Cast<Task>()); // Wait for all subsector generation tasks to complete
        
        
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Finished generating {Name} Sector");
                Console.ForegroundColor = ConsoleColor.Gray;
        }

        return Sector;
    }

    private async Task<KURPGSubsector> GenerateSectorData(int x, int y)
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Beginning generation of {Name} {x} {y} Subsector");        
            Console.ForegroundColor = ConsoleColor.Gray;

        }
                var generator = new KURPGSubsectorGenerator(Name + $" {x},{y} Subsector", UsingSeed, Seed + x + y, IsPrinting);
                var result = generator.Generate();

                if (Sector != null) Sector.Subsectors[x, y] = await result;

                if (IsPrinting)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Finished generation of {Name} {x} {y} Subsector");        
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
                return await result;
    }
}