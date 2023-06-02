namespace Simple_Subsector_Generator;

public abstract class KURpgStarSystem
{
    public int SystemX { get; }
    public int SystemY { get; }
    

    public KURpgStarSystem(int x, int y)
    {
        SystemX = x;
        SystemY = y;
    }

    public override string ToString()
    {
        return $"## {SystemX+1}:{SystemY+1} ";
    }
}