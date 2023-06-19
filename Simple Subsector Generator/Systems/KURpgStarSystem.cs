namespace Simple_Subsector_Generator;

public abstract class KURPGStarSystem
{
    public int SystemX { get; }
    public int SystemY { get; }
    

    public KURPGStarSystem(int x, int y)
    {
        SystemX = x;
        SystemY = y;
    }

    public override string ToString()
    {
        return $"## {SystemX+1}:{SystemY+1} ";
    }
}