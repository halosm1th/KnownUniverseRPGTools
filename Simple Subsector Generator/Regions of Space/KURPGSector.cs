using System.Text;

namespace Simple_Subsector_Generator;

public class KURPGSector
{
    public KURPGSubsector[,] Subsectors { get; }
    public string Name { get; }
    public int Seed { get; }

    public KURPGSector(string name, int seed)
    {
        Name = name;
        Seed = seed;
        Subsectors = new KURPGSubsector[4, 4];
    }

    public override string ToString()
    {
        return $"# {Name} Sector ({Subsectors.GetLength(0)+1}x{Subsectors.GetLength(1)+1})\n" +
               $"{SubsectorsText()}";
    }

    private string SubsectorsText()
    {
        var sb = new StringBuilder();
        foreach (var s in Subsectors)
        {
            sb.Append(s.ToString());
        }

        return sb.ToString();
    }
}
