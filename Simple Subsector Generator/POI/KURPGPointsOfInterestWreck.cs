class KURPGPointsOfInterestWreck: KURPGPointsOfInterest
{
    public int SizeRoll { get; set; }
    public int AliensInsideRoll { get; set; }
    public int LootedRoll { get; set; }
    public override bool HasComplexInfo => true;

    public override string ToString()
    {
        return base.ToString() + $"\n\t- Size: {SizeMeaning}, Aliens Inside: {AliensMeaning}," +
               $" Percentage Looted: {LootedMeaning}\n";
    }
    
    
    public override List<KURPGTradeCodes> GetTradeCodes()
    {
        return new List<KURPGTradeCodes>();
    }

    public string SizeMeaning => SizeRoll switch
    {
        <=1 => "Tiny",
        2 => "Tiny",
        3 => "Extra Small",
        4 => "Small",
        5 => "Small",
        6 => "Average",
        7 => "Average",
        8 => "Average",
        9 => "Large",
        10 => "Large",
        11 => "Extra Large",
        12 => "Massive",
        13 => "Super-Massive",
        >=14 => "Super-Massive"
    };
    
    
    public string AliensMeaning => AliensInsideRoll switch
    {
        <=1 => "Yes - 2D20",
        2 => "Yes - 1D20",
        3 => "Yes - 1D12",
        4 => "Yes - 1D10",
        5 => "Yes - 1D8",
        6 => "Yes - 1D6",
        7 => "Yes - 1D4",
        8 => "Yes - 1D3",
        9 => "No",
        10 => "No",
        11 => "No",
        12 => "No",
        13 => "No",
        >=14 => "Yes - Special/Exotic"
    };
    
    
    public string LootedMeaning => LootedRoll switch
    {
        <=1 => "100%",
        2 => "100%",
        3 => "90%",
        4 => "80%",
        5 => "70%",
        6 => "60%",
        7 => "50%",
        8 => "40%",
        9 => "30%",
        10 => "20%",
        11 => "10%",
        12 => "0%",
        13 => "0%",
        >=14 => "0%"
    };
    
    public KURPGPointsOfInterestWreck( int subtypeRoll) : base(KURPGPoiTypes.Wreck, subtypeRoll)
    {
    }

    public override string? SubtypeName  => SubtypeRoll switch
    {
        1 => "Ruins from an ancient battle",
        2 => "Ruins from a recent battle",
        3 => "Ruins from a pirate attack",
        4 => "Long destroyed station",
        5 => "Remains of a super-massive ships",
        6 => "Remains of a treasure fleet",
        7 => "Ruins of a failed colonization mission",
        8 => "Ruins of a religious fleet",
        _ => throw new ArgumentOutOfRangeException()
    };
    public override string SubTypeDescription  => SubtypeRoll switch
    {
        1 => "There are ruins from an ancient battle dating back to before humanity existed.",
        2 => "There are ruins from a relatively recent battle.",
        3 => "There are ruins from a relatively recent pirate attack.",
        4 => "This was once a station, but that was many millenium ago.",
        5 => "This was once a super-massive ship, like a world ship or Galtran which has been destroyed.",
        6 => "This was a treasure fleet which was destroyed for some reason. Perhaps pirates, perhaps something else.",
        7 => "This was once a ship or fleet which was going out to colonize a planet, system, or more.",
        8 => "This was once a ship or fleet from some religion going on a pilgrimage, fleeing a planet, or going on some other journey.",
        _ => throw new ArgumentOutOfRangeException()
    };
}