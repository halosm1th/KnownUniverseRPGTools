using System.Text;

namespace Simple_Subsector_Generator;

public class KURPGSector
{
    public KURPGSubsector[,] Subsectors { get; }


    private List<KURPGPointsOfInterest> _PointsOfInterests;
    public List<KURPGPointsOfInterest> PointsOfInterests
    {
        get
        {
            
            var poi = new List<KURPGPointsOfInterest>();
            foreach (var kurpgSubsector in Subsectors)
            {
                var items = kurpgSubsector.GetFilledSystems().Select(x => x?.PointsOfInterest);
                foreach (var item in items)
                {
                    poi.AddRange(item);   
                }
            }

            return poi;
        }
    }

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
