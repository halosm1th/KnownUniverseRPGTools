namespace KUP_Simple_Sector_Generator;

public abstract class KUPStarSystem
{
    public int SystemX { get; }
    public int SystemY { get; }
    
    public KUPSubsector Subsector { get; }
    
    public int DisplayX { get; }
    public int DisplayY { get; }

    public KUPStarSystem(int x, int y, KUPSubsector subsector, ref int dispX, ref int dispY)
    {
        SystemX = x;
        SystemY = y;
        Subsector = subsector;
        DisplayX = dispY;
        DisplayY = dispX;
    }

    public override string ToString()
    {
        return $"## {SystemX+1}:{SystemY+1} ";
    }
}