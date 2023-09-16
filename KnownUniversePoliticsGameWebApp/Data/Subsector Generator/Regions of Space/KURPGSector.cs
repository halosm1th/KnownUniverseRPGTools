using System.Text;

namespace Simple_Subsector_Generator;

public class KURPGSector
{
    public KURPGSubsector[,] Subsectors { get; }

    public List<KUPFilledSystem> FilledSystems
    {
        get
        {
            var systems = new List<KUPFilledSystem>();
            foreach (var sub in Subsectors)
            {
                systems.AddRange(sub.GetFilledSystems());
            }

            return systems;
        }
    }


    private List<KUPPointsOfInterest> _PointsOfInterests;

    public List<KUPPointsOfInterest> PointsOfInterests
    {
        get
        {

            var poi = new List<KUPPointsOfInterest>();
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

    public List<KUPStarSystem> GetSystems()
    {
        var results = new List<KUPStarSystem>();

        foreach (var sub in Subsectors)
        {
            results.AddRange(sub.Systems());
        }
        
        return results;
    }

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
