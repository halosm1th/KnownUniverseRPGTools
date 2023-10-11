using System.Text;
using KUP_Simple_Sector_Generator;

namespace KnownUniversePoliticsGameWebApp.Data.Subsector_Generator.Regions_of_Space;

public class KURPGSuperSector
{
    public KUPSector?[,] Sectors { get; }
    public string Name { get; }
    public int Seed { get; }
    public bool UsingSeed { get; }

    public KURPGSuperSector(string name, int seed)
    {
        Name = name;
        Seed = seed;
        Sectors = new KUPSector?[4, 4];
    }


    public override string ToString()
    {
        return $"# {Name} Sector ({Sectors.GetLength(0)+1}x{Sectors.GetLength(1)+1})\n" +
               $"{SectorsText()}";
    }

    private string SectorsText()
    {
        var sb = new StringBuilder();
        foreach (var s in Sectors)
        {
            sb.Append(s.ToString());
        }

        return sb.ToString();
    }
}