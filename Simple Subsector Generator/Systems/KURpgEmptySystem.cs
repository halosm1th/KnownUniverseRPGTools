using Simple_Subsector_Generator;

class KURPGEmptySystem : KURPGStarSystem
{
    public KURPGEmptySystem(int x, int y) : base(x, y)
    {
    }

    public override string ToString()
    {
        return "";
        return base.ToString() + "**An Empty System**";
    }
}