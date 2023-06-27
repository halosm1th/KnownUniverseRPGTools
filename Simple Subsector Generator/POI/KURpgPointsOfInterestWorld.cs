public class KURPGPointsOfInterestWorld : KURPGPointsOfInterest
{
    public int TLRoll { get; set; }
    public int PopulationRangeRoll { get; set; }
    public int LawLevelRoll { get; set; }
    public int GovernemntRoll { get; set; }
    public int SizeRoll { get; set; }
    public int AtmosphereRoll { get; set; }
    public int GravityRoll { get; set; }

    public override string ToString()
    {
        return base.ToString() +
               $"\n- **World Info** - TL: {TLMeaning}, Pop: {PopulationMeaning}, Law Level: {LawLevelMeaning} " +
               $"Government: {GovernmentMeaning}, Size: {SizeMeaning}, Atmosphere: " +
               $"{AtmosphereMeaning}, Gravity: {GravityMeaning}\n- **Trade Codes**: {GetTradecodeMeaning()}.\n";
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
            tags.Add(KURPGTradeCodes.Ha);
        }else if (SubtypeRoll == 2)
        {
            tags.Add(KURPGTradeCodes.Ag);

        }else if (SubtypeRoll == 3)
        {
            tags.Add(KURPGTradeCodes.Ma);

        }else if (SubtypeRoll == 4)
        {
            tags.Add(KURPGTradeCodes.Ec);

        }else if (SubtypeRoll == 5)
        {
            tags.Add(KURPGTradeCodes.Re);

        }else if (SubtypeRoll == 6)
        {
            tags.Add(KURPGTradeCodes.Ga);

        }else if (SubtypeRoll == 7)
        {
            tags.Add(KURPGTradeCodes.Gw);

        }else if (SubtypeRoll == 8)
        {
            tags.Add(KURPGTradeCodes.Rl);
        }

        return tags;
    }
    
    public override bool HasComplexInfo => true;
    public string GravityMeaning => GravityRoll switch
    {

        <=1 => "0 (0)",
        2 => "0 (0)",
        3 => "0.05 (1)",
        4 => "0.15",
        5 => "0.25",
        6 => "0.35",
        7 => "1.0",
        8 => "1.0",
        9 => "0.45",
        10 => "0.7",
        11 => "0.9",
        12 => "1.25",
        13 => "1.4",
        >=14 => "Inverted"
    };
    
    public string AtmosphereMeaning => AtmosphereRoll switch
    {

        <=1 => "None: Space Suit required (0)",
        2 => "None: Space Suit required (0)",
        3 => "Traces: Space Suit required (1)",
        4 => "Thin: Respirator required (2)",
        5 => "Thin, contaminated: Filter, Respiratorrequired (3)",
        6 => "Thin: Respirator required (2)",
        7 => "Standard: Nothing (4)",
        8 => "Standard: Nothing (4)",
        9 => "Standard, contaminated: Filter Required (5)",
        10 => "Dense (6)",
        11 => "Low: Respirator recommended (7)",
        12 => "Acidic: Space Suit Required (8)",
        13 => "Magical: May require Magical protections (9)",
        >=14 => "Other: May require Respirator, Filter, or Space Suit (A)"
    };
    
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
    
    public KURPGPointsOfInterestWorld(int subtypeRoll, KURPGFilledSystem system) : base(KURPGPoiTypes.Planet, subtypeRoll, system)
    {
    }

    public override string? SubtypeName => SubtypeRoll switch
    {
        1 => "Inhabited World / Government World",
        2 => "Agricultural World",
        3 => "Manufacturing World",
        4 => "Economic World",
        5 => "Resource World",
        6 => "Gas Giant",
        7 => "Government World",
        8 => "Religious World",
        _ => throw new ArgumentOutOfRangeException()
    };
    public override string SubTypeDescription  => SubtypeRoll switch
    {
        1 => "A normal world, does some food, some industry, some economics, some resources, and some government work.",
        2 => "a major source of food and other food-goods.",
        3 => "A major source of industry and production of goods.",
        4 => "A major economic hub.",
        5 => "A world full of one or more types of raw resources to be exploited.",
        6 => "A gas giant of some kind.",
        7 => "A world which serves the government fully.",
        8 => "A world devote to a religion.",
        _ => throw new ArgumentOutOfRangeException()
    };
}