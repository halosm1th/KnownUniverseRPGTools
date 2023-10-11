using System.Text;

namespace KUP_Simple_Sector_Generator;

public class KURPGSectorGenerator
{
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }

    public bool IsPrinting = false;

    public KURPGSector? Sector { get; private set; }

    public KURPGSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting = false)
    {
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        Name = KURpgSubsectorGenerator.GetProvinceName(Seed);
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
        if (Sector != null)
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
    }

    public KURPGSector Generate()
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Starting to generate {Name} Sector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        Sector = new KURPGSector(Name, Seed);

        for(var x = 0; x < Sector.Subsectors.GetLength(0); x++)
        {
            for(var y = 0; y < Sector.Subsectors.GetLength(1); y++)
            {
                _Index++;
                Sector.Subsectors[x,y] = GenerateSectorData(x, y,_Index);
            }
        }
        
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Finished generating {Name} Sector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        return Sector;
    }
    
    public async Task<KURPGSector?> GenerateAsync()
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
                _Index++;
                tasks[x, y] = GenerateSectorDataAsync(x, y,_Index);
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

    public static int _Index = 0;
    
    private KURPGSubsector GenerateSectorData(int x, int y, int index)
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Begining generation of {Name} {x} {y} Subsector");        
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        var name = Name + $" {x},{y}  "+ KURpgSubsectorGenerator.GetName(Seed);
        var seed = Seed + index;
        var generator = new KURpgSubsectorGenerator(name,UsingSeed,seed , IsPrinting);
        var result = generator.Generate();

        if (Sector != null) Sector.Subsectors[x, y] = result;

        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Finished generation of {Name} {x} {y} Subsector");        
            Console.ForegroundColor = ConsoleColor.Gray;

        }
        return result;
    }
    
    private async Task<KURPGSubsector> GenerateSectorDataAsync(int x, int y, int index)
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Begining generation of {Name} {x} {y} Subsector");        
            Console.ForegroundColor = ConsoleColor.Gray;

        }

                var name = Name + $" {x},{y}  "+ KURpgSubsectorGenerator.GetName(Seed);
                var seed = Seed + index;
                var generator = new KURpgSubsectorGenerator(name,UsingSeed,seed , IsPrinting);
                var result = generator.GenerateAsync();

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