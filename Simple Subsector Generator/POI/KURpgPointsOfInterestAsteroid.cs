class KURpgPointsOfInterestAsteroid: KURpgPointsOfInterest
{
    public int TLRoll { get; set; }
    public int PopulationRangeRoll { get; set; }
    public int LawLevelRoll { get; set; }
    public int GovernemntRoll { get; set; }
    public int SizeRoll { get; set; }

    public override bool HasComplexInfo => true;
    public KURpgPointsOfInterestAsteroid(int subtypeRoll) : base(KURpgPoiTypes.Asteroid, subtypeRoll)
    {
    }

    public override List<KURPGTradeCodes> GetTradeCodes()
    {
        var tags = new List<KURPGTradeCodes>();
        if (TLRoll >= 11)
        {
            tags.Add(KURPGTradeCodes.Ht);
        }else if (TLRoll <= 4)
        {
            tags.Add(KURPGTradeCodes.Lt);
        }

        if (PopulationRangeRoll >= 11)
        {
            tags.Add(KURPGTradeCodes.Hp);
        }else if (PopulationRangeRoll <= 5)
        {
            tags.Add(KURPGTradeCodes.Lp);
        }
        
        
        if (LawLevelRoll >= 8)
        {
            tags.Add(KURPGTradeCodes.Hl);
        }else if (PopulationRangeRoll <= 5)
        {
            tags.Add(KURPGTradeCodes.Ll);
        }

        if (SubtypeRoll == 1)
        {
            tags.Add(KURPGTradeCodes.Re);
        }else if (SubtypeRoll == 3)
        {
            tags.Add(KURPGTradeCodes.Io);

        }else if (SubtypeRoll == 5)
        {
            tags.Add(KURPGTradeCodes.Ha);

        }else if (SubtypeRoll == 8)
        {
            tags.Add(KURPGTradeCodes.Ms);
        }

        return tags;
    }

    public override string ToString()
    {
        return base.ToString() + $"\n- **Asteroid Info** - TL: {TLMeaning}, Pop: {PopulationMeaning}, Law Level: {LawLevelMeaning}" +
               $" Government: {GovernmentMeaning}, Size: {SizeMeaning}\n- **Trade Codes**: {GetTradecodeMeaning()}.\n";
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
    
    public string GovernmentMeaning => GovernemntRoll switch
    {

        <=1 => "No Government (0)",
        2 => "Balkanized States (1)",
        3 => "Religious Autocracy (2)",
        4 => "Direct Democracy (3)",
        5 => "Representative Democracy (4)",
        6 => "Multi-Corporate Control (5)",
        7 => "Functional bureaucracy (6)",
        8 => "Corpo-Feudal-technocracy (7)",
        9 => "Captive Government (8)",
        10 => "Charismatic Oligarchy (9)",
        11 => "Dysfunctional bureaucracy (A)",
        12 => "Charismatic Autocracy (B)",
        13 => "Oppressive Dictatorship (C)",
        >=14 => "Civil Service bureaucracy (D)"
    };
    
    public string LawLevelMeaning => LawLevelRoll switch
    {

        <=1 => "None: No Weapons or Armour are Banned",
        2 => "War Crimes: Weapons of mass destruction, poising gas, etc are banned",
        3 => "Military Weapons banned. Military and Special Forces quality weapons are banned",
        4 => "Heavy Weapons banned. All light machine guns, most sub machine guns, rocket launchers, plasma weapons, etc are banned.",
        5 => "Carrying Weapons discouraged, all automatic, plasma, and high caliber weapons are banned from being carried.",
        6 => "Carrying Sidearms discouraged, all sidearms, small arms, etc are discouraged from being carried. Automatics and plasma weapons are often banned.",
        7 => "Shotguns heavily discouraged, handguns and automatics banned. Some models of weapons might still sneak through.",
        8 => "Guns banned: all Firearms are banned, melee weapons are restricted.",
        9 => "All Blades Banned: All Bladed melee weapons are banned.",
        10 => "All Melee Banned: All guns and melee weapons are banned.",
        11 => "Everything: Everything, all weapons and armour are banned to the best of the worlds abilities. People with these items on them are prosecuted heavily.",
        12 => "Everything: Everything, all weapons and armour are banned to the best of the worlds abilities. People with these items on them are prosecuted heavily.",
        13 => "Everything: Everything, all weapons and armour are banned to the best of the worlds abilities. People with these items on them are prosecuted heavily.",
        >=14 => "Everything: Everything, all weapons and armour are banned to the best of the worlds abilities. People with these items on them are prosecuted heavily."
    };
    
    public int GetLawLevelModifier => LawLevelRoll switch
    {
        
        <=1 => -3,
        2 => -2,
        3 => -2,
        4 => -1,
        5 => -1,
        6 => 0,
        7 => 0,
        8 => 0,
        9 => 1,
        10 => 2,
        11 => 3,
        12 => 3,
        13 => 3,
        >=14 => 3
    };
    
    public string PopulationMeaning => PopulationRangeRoll switch
    {

        <=1 => "1-10 People",
        2 => "10-50 People",
        3 => "50-500 People",
        4 => "500-1000",
        5 => "1-10 Thousand",
        6 => "10-100 Thousand ",
        7 => "100-500 Thousand",
        8 => "500-999 Thousand ",
        9 => "1-10 Million",
        10 => "10-100 Million",
        11 => "100-900 Million",
        12 => "1-90 Billion",
        13 => "100-900 Billion",
        >=14 => "1-10 Trillion"
    };
    
    public int GetPopModifier => PopulationRangeRoll switch
    {
        
        <=1 => -3,
        2 => -3,
        3 => -2,
        4 => -2,
        5 => -1,
        6 => -1,
        7 => 0,
        8 => 0,
        9 => 0,
        10 => 1,
        11 => 1,
        12 => 1,
        13 => 2,
        >=14 => 3
    };
    
    public int GetTLModifier => TLRoll switch
    {
        
        <=1 => -3,
        2 => -2,
        3 => -1,
        4 => 0,
        5 => 0,
        6 => 0,
        7 => 1,
        8 => 1,
        9 => 1,
        10 => 2,
        11 => 2,
        12 => 2,
        13 => 3,
        >=14 => 3
    };
    
    public string TLMeaning => TLRoll switch
    {
        <=1 => "1-2 ",
        2 => "2-4",
        3 => "5-7",
        4 => "8",
        5 => "9",
        6 => "10 (A)",
        7 => "11 (B)",
        8 => "12 (C)",
        9 => "13 (D)",
        10 => "14 (E)",
        11 => "15 (F)",
        12 => "16 (G)",
        13 => "17 (H)",
        >=14 => "18 (I)"
        
    };
    
    public override string? SubtypeName  => SubtypeRoll switch
    {
        1 => "Resource Extraction operation",
        2 => "Someone's Hidden Base",
        3 => "Illegal Trade Outpost",
        4 => "A privately owned asteroid",
        5 => "A habitation station",
        6 => "hidden research lab",
        7 => "Black Ops Site",
        8 => "Military Base",
        _ => throw new ArgumentOutOfRangeException()
    };
    public override string SubTypeDescription  => SubtypeRoll switch
    {
        1 => "Some kind of raw resource such as ice, iron, gold, uranium, or something else is being mined on this asteroid.",
        2 => "Someone or some group has hidden a base on an asteroid.",
        3 => "An illegal trade outpost has been setup on this asteroid.",
        4 => "This asteroid is privately owned by someone.",
        5 => "This asteroid has been converted in one or more series of habitats for people to live in.",
        6 => "This asteroid contains a hidden research lab.",
        7 => "This asteroid has been turned into a black ops site for some covert purpose.",
        8 => "This asteroid is a military base.",
        _ => throw new ArgumentOutOfRangeException()
    };
}