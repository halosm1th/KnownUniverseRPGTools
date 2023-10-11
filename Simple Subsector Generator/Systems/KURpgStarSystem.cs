namespace KUP_Simple_Sector_Generator;

public abstract class KURPGStarSystem
{
    public int SystemX { get; }
    public int SystemY { get; }
    
    public KURPGSubsector Subsector { get; }
    

    public KURPGStarSystem(int x, int y, KURPGSubsector subsector)
    {
        SystemX = x;
        SystemY = y;
        Subsector = subsector;
    }

    public override string ToString()
    {
        return $"## {SystemX+1}:{SystemY+1} ";
    }
}