namespace Simple_Subsector_Generator;

public abstract class KURPGStarSystem
{
    public int SystemX { get; }
    public int SystemY { get; }
    
    public KURPGSubsector Subsector { get; }
    
    public int DisplayX { get; }
    public int DisplayY { get; }

    public KURPGStarSystem(int x, int y, KURPGSubsector subsector, ref int dispX, ref int dispY)
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