using Simple_Subsector_Generator;

public class KURPGEmptySystem : KURPGStarSystem
{
    public KURPGEmptySystem(int x, int y, KURPGSubsector subsector) : base(x, y, subsector)
    {
    }

    public override string ToString()
    {
        return "";
        return base.ToString() + "**An Empty System**";
    }
}