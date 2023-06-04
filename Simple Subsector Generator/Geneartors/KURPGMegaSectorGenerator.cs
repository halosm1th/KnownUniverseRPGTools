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
            //Console.WriteLine($"Writing: ("+ s + ") to file.");
            sw.Flush();
        }
        sw.Close();
        fs.Close();
    }


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
        /*
        var tasks = new List<Task<KURPGSuperSector>>();
        for (var x = 0; x < MegaSector.SuperSectors.GetLength(0); x++)
        {
            for (var y = 0; y < MegaSector.SuperSectors.GetLength(1); y++)
            {
                var task = GenerateMegaSectorData(x, y);
                //task.Start();
                tasks.Add(task);
            }  
        }
        
        var x1 = 0;
        var y1 = 0;
        foreach (var task in tasks)
        {
            if(IsPrinting) Console.WriteLine($"Is stashing result for {Name} {x1} {y1}");
            MegaSector.SuperSectors[x1, y1] = await task;
            if (y1 < MegaSector.SuperSectors.GetLength(0)-1) y1++;
            else
            {
                y1 = 0;
                if (x1 < MegaSector.SuperSectors.GetLength(1)-1) x1++;
            }
        }


        if(IsPrinting) Console.WriteLine($"Finished generating {Name} SuperSector");

        Console.WriteLine("Waiting for you to press the button to finish");
        
        return MegaSector;*/
    }
    
    private async Task<KURPGSuperSector?> GenerateMegaSectorData(int x, int y)
    {
       // if(IsPrinting) Console.WriteLine($"Beginning generation of {Name} {x} {y} Sector");
        var generator = new KURPGSuperSectorGenerator(Name + $" {x},{y} Super Sector", UsingSeed, Seed + x + y, IsPrinting);
        var result = generator.Generate();

        if (MegaSector != null) MegaSector.SuperSectors[x, y] = await result;

        return await result;
    }

   /* public void Generate()
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
    }*/
}