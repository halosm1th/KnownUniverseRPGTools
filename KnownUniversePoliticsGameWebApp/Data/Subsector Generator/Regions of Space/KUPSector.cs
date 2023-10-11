using System.Text;
using KUP_Simple_Sector_Generator;

namespace KnownUniversePoliticsGameWebApp.Data.Subsector_Generator.Regions_of_Space;

public class KUPSector
{
    public KUPSubsector[,] Subsectors { get; }

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

    public string GetSystemName(int locX, int locY)
    {
        var sys = GetSystems().First(x => x.DisplayX == locX && x.DisplayY == locY);
        if (sys.GetType() == typeof(KUPFilledSystem))
        {
            return ((KUPFilledSystem) sys).Name;
        }
        
        return "Empty Space";
    }

    public List<KUPStarSystem> GetSystems()
    {
        var results = new List<KUPStarSystem>();

        foreach (var sub in Subsectors)
        {
            results.AddRange(sub.Systems());
        }
        
        return results;
    }

    public KUPSector(string name, int seed)
    {
        Name = name;
        Seed = seed;
        Subsectors = new KUPSubsector[4, 4];
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
