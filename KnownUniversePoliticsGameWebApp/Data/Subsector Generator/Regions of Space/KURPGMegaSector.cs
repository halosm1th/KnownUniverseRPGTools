using System.Text;
using KUP_Simple_Sector_Generator;

namespace KnownUniversePoliticsGameWebApp.Data.Subsector_Generator.Regions_of_Space;

public class KURPGMegaSector
{
    public KURPGSuperSector?[,] SuperSectors { get; }
    public string Name { get; }
    public int Seed { get; }
    public bool UsingSeed { get; }

    public KURPGMegaSector(string name, int seed)
    {
        Name = name;
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        SuperSectors = new KURPGSuperSector?[6, 6];
    }


    public override string ToString()
    {
        return $"# {Name} Sector ({SuperSectors.GetLength(0)+1}x{SuperSectors.GetLength(1)+1})\n" +
               $"{SectorsText()}";
    }

    private string SectorsText()
    {
        var sb = new StringBuilder();
        foreach (var s in SuperSectors)
        {
            sb.Append(s?.ToString());
        }

        return sb.ToString();
    }
}
