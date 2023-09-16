using System.Text;

namespace Simple_Subsector_Generator;

class KURPGMegaSectorGenerator
{

    public string Name { get; }
    public int Seed { get; }
    public bool UsingSeed { get; }
    public bool IsPrinting = false;
    public KURPGMegaSector? MegaSector { get; private set; }

    public KURPGMegaSectorGenerator(string name, bool usingSeed, int seed, bool isPrinting)
    {
        Name = name;
        Seed = seed;
        UsingSeed = usingSeed;
        IsPrinting = isPrinting;
        MegaSector = null;
    }

    // can be in parent
    public void WriteToFile(string name, string path)
    {
        var utf8Text = GeneratorUtils.Utf8Encoder.GetString(
            GeneratorUtils.Utf8Encoder.GetBytes(MegaSector.ToString()));

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

    // can be in parent, pass in colour
    // Can all sectors generators be ran asynch?
    public async Task<KURPGMegaSector?> GenerateAsync()
    {
        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Starting to generate {Name} Megasector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        MegaSector= new KURPGMegaSector(Name,Seed);

        var tasks = new Task[MegaSector.SuperSectors.GetLength(0),
            MegaSector.SuperSectors.GetLength(1)];

        Parallel.For(0, MegaSector.SuperSectors.GetLength(0), x =>
        {
            Parallel.For(0, MegaSector.SuperSectors.GetLength(1), y =>
            {
                tasks[x, y] = GenerateMegaSectorData(x, y);
            });
        });

        await Task.WhenAll(tasks.Cast<Task>()); // Wait for all subsector generation tasks to complete

        if (IsPrinting)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Finished generating {Name} Megasector");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        return MegaSector;
    }

    // make this one mandatory and implement in each thing
    private async Task<KURPGSuperSector?> GenerateMegaSectorData(int x, int y)
    {
       // if(IsPrinting) Console.WriteLine($"Beginning generation of {Name} {x} {y} Sector");
        var generator = new KURPGSuperSectorGenerator(Name + $" {x},{y} Super Sector", UsingSeed, Seed + x + y, IsPrinting);
        var result = generator.GenerateAsync();

        if (MegaSector != null) MegaSector.SuperSectors[x, y] = await result;

        return await result;
    }

}
