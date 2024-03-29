﻿using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace KUP_Simple_Sector_Generator;

public class KUPSubsector
{
    public Dictionary<(int X, int Y),KUPStarSystem> Subsector { get; private set; }

    public string Name { get; }
    public int XSize { get; }
    public int YSize { get; }
    
    public int SubsectorX { get; }
    public int SubsectorY { get; }
    
    


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
    
    public KUPSubsector(string name, int xSize, int ySize, int subX, int subY)
    {
        //Y,X
        Subsector = new Dictionary<(int X, int Y), KUPStarSystem>();
        Name = name;
        XSize = xSize;
        YSize = ySize;
        SubsectorX = subX;
        SubsectorY = subY;
    }

    public KUPFilledSystem? GetFilledSystem(int x, int y)
    {
        if (IsFilledSystem(x, y))
            return Subsector[(x, y)] as KUPFilledSystem;
        return null;
    }
    
    
    public List<KUPStarSystem> Systems()
    {
        return Subsector.Select(x => x.Value).ToList();
    }
    
    public KUPStarSystem GetSystem(int x, int y)
    {
        return Subsector[(x, y)];
    }

    public List<KUPFilledSystem?> GetFilledSystems()
    {
        return Subsector.Where(x => x.Value.GetType() == typeof(KUPFilledSystem))
            .Select(x => x.Value as KUPFilledSystem).ToList();
    }
    
    public void PlaceSystem(KUPStarSystem system, int x, int y)
    {
        Subsector[(x, y)] = system;
    }

    public bool IsFilledSystem(int x, int y)
    {
        return !IsEmptySystem(x,y);
    }

    public bool IsEmptySystem(int x, int y)
    {
        return Subsector[(x, y)].GetType() == typeof(KupEmptySystem);
    }

}
