using KUP_Simple_Sector_Generator;

public class KURPGEmptySystem : KURPGStarSystem
{
    public KURPGEmptySystem(int x, int y, KURPGSubsector subsector) : base(x, y, subsector)
    {
    }

    public override string ToString()
    {
        return "";
    }
}