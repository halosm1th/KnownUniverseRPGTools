using Simple_Subsector_Generator;

class KURpgEmptySystem : KURpgStarSystem
{
    public KURpgEmptySystem(int x, int y) : base(x, y)
    {
    }

    public override string ToString()
    {
        return base.ToString() + "**An Empty System**";
    }
}