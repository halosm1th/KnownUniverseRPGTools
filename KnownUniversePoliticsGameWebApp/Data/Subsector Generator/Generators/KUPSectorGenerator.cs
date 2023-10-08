using System.Text;
using KnownUniversePoliticsGameWebApp.Data;
using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace Simple_Subsector_Generator;

public class KUPSectorGenerator
{
    public string Name { get; }
    public int Seed { get; }
    
    public bool UsingSeed { get; }

    public bool IsPrinting = false;
    private int DisplayX;
    private int DisplayY;

    public KURPGSector? Sector { get; private set; }
    private KnownUniversePoliticsGame PoliticsGame;

    public KUPSectorGenerator(string name, bool usingSeed, int seed,KnownUniversePoliticsGame politicsGame ,
        bool isPrinting = false, int dispX = 1, int dispY =1)
    {
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        Name = KUPSubsectorGenerator.GetProvinceName(Seed);
        UsingSeed = usingSeed;
        IsPrinting = isPrinting;
        PoliticsGame = politicsGame;
        DisplayX = dispX;
        DisplayY = dispY;
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
                DisplayY += 10;
            }

            DisplayY = 1;
            DisplayX = DisplayX += 8;
        }
        
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

        var name = Name + $" {x},{y}  "+ KUPSubsectorGenerator.GetName(Seed);
        var seed = Seed + index;
        var generator = new KUPSubsectorGenerator(name, PoliticsGame, DisplayX, DisplayY,UsingSeed,seed , IsPrinting,x,y);
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
    
}