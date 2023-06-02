using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Simple_Subsector_Generator;

public class KURpgSubsector
{
    public Dictionary<(int X, int Y),KURpgStarSystem> Subsector { get; private set; }

    public string Name { get; }
    public int XSize { get; }
    public int YSize { get; }


    private string SubsectorText()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var system in Subsector)
        {
            sb.Append(system.Value.ToString());
            sb.Append("\n");
        }

        return sb.ToString();
    }
    
    public override string ToString()
    {
        return $"# {Name} Subsector ({XSize}x{YSize})\n" +
               $"{SubsectorText()}";
        
    }
    
    public KURpgSubsector(string name, int xSize, int ySize)
    {
        //Y,X
        Subsector = new Dictionary<(int X, int Y), KURpgStarSystem>();
        Name = name;
        XSize = xSize;
        YSize = ySize;
    }

    public KURpgFilledSystem? GetFilledSystem(int x, int y)
    {
        if (IsFilledSystem(x, y))
            return Subsector[(x, y)] as KURpgFilledSystem;
        return null;
    }
    
    public KURpgStarSystem GetSystem(int x, int y)
    {
        return Subsector[(x, y)];
    }

    public List<KURpgFilledSystem?> GetFilledSystems()
    {
        return Subsector.Where(x => x.Value.GetType() == typeof(KURpgFilledSystem))
            .Select(x => x.Value as KURpgFilledSystem).ToList();
    }
    
    public void PlaceSystem(KURpgStarSystem system, int x, int y)
    {
        Subsector[(x, y)] = system;
    }

    public bool IsFilledSystem(int x, int y)
    {
        return !IsEmptySystem(x,y);
    }
    
    public bool IsEmptySystem(int x, int y)
    {
        return Subsector[(x, y)].GetType() == typeof(KURpgEmptySystem);
    }
}