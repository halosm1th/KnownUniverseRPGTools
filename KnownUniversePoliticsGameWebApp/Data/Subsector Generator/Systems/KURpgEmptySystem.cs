using Simple_Subsector_Generator;

public class KURPGEmptySystem : KURPGStarSystem
{
    public KURPGEmptySystem(int x, int y, KURPGSubsector subsector, ref int dispX, ref int dispY) 
        : base(x, y, subsector, ref dispX,ref dispY)
    {
    }

    public override string ToString()
    {
        return "";
        return base.ToString() + "**An Empty System**";
    }
}