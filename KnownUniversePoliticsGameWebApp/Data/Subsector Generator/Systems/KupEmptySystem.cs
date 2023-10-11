using KUP_Simple_Sector_Generator;

public class KupEmptySystem : KUPStarSystem
{
    public KupEmptySystem(int x, int y, KUPSubsector subsector, ref int dispX, ref int dispY) 
        : base(x, y, subsector, ref dispX,ref dispY)
    {
    }

    public override string ToString()
    {
        return "";
        return base.ToString() + "**An Empty System**";
    }
}