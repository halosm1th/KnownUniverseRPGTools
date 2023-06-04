class KURPG_PointsOfInterest_Other: KURpgPointsOfInterest
{
    public int TL { get; }
    public int PopulationRangeRoll { get; }
    public int LawLevelRoll { get; }
    public int GovernemntRoll { get; }
    public int SizeRoll { get; }
    public override bool HasComplexInfo => false;

    public override List<KURPGTradeCodes> GetTradeCodes()
    {
        return new List<KURPGTradeCodes>();
    }

    public override string ToString()
    {
        var text = base.ToString().Remove(base.ToString().Length-5,5);
        return text + "\n";
    }
    public KURPG_PointsOfInterest_Other(int subtypeRoll) : base(KURpgPoiTypes.Other, subtypeRoll)
    {
    }

    public override string? SubtypeName  => SubtypeRoll switch
    {
        1 => "Space Casino",
        2 => "System Defense Station",
        3 => "Gravity Amusement Park",
        4 => "Void-Gate",
        5 => "Space Creature present",
        6 => "Ancient Space Station",
        7 => "Terrorist Hideout",
        8 => "Black Market Hub",
        _ => throw new ArgumentOutOfRangeException()
    };
    public override string SubTypeDescription  => SubtypeRoll switch
    {
        1 => "A Casino somewhere in space, perhaps a station, an asteroid, on a planet, or some other exotic location.",
        2 => "A weapons platform in space dedicated to defending the system.",
        3 => "An amusement park based on the idea of manipulating gravity to perform otherworldly attraction design.",
        4 => "A stellar-void gate for moving ships between sectors or greater.",
        5 => "A space creature of some kind is present in this system.",
        6 => "A space station which dates back to before humanity existed is present in this system.",
        7 => "A group of terrorists or rebels have built a base somewhere in this system.",
        8 => "There is a major black market hub in this system.",
        _ => throw new ArgumentOutOfRangeException()
    };
}