class KURPGPointOfInterestAnomaly : KURPGPointsOfInterest
{
    public KURPGPointOfInterestAnomaly(int subtypeRoll) : base(KURPGPoiTypes.Anomaly, subtypeRoll)
    {
    }
    public override bool HasComplexInfo => false;

    public override string ToString()
    {
        var text = base.ToString().Remove(base.ToString().Length-5,5);
        return text + "\n";
    }
    
    
    public override List<KURPGTradeCodes> GetTradeCodes()
    {
        var codes = new List<KURPGTradeCodes>();
        
        if (SubtypeRoll == 1)
        {
            codes.Add(KURPGTradeCodes.Sc);

        }else if (SubtypeRoll == 2)
        {
            codes.Add(KURPGTradeCodes.Mb);

        }else if (SubtypeRoll == 4)
        {
            codes.Add(KURPGTradeCodes.Vg);

        }else if (SubtypeRoll == 8)
        {
            codes.Add(KURPGTradeCodes.Wh);

        }
        
        return codes;
    }

    public override string? SubtypeName  => SubtypeRoll switch
    {
        1 => "Wormhole",
        2 => "Nebula",
        3 => "Black Hole",
        4 => "Rogue Planet",
        5 => "Gravity Well",
        6 => "Gravity Anomaly",
        7 => "Gravity Anomaly",
        8 => "Planar Rift",
        _ => throw new ArgumentOutOfRangeException()
    };
    public override string SubTypeDescription  => SubtypeRoll switch
    {
        1 => "A spatial anomaly that connects two distant points in space, allowing for near-instantaneous travel between them. If you roll another wormhole, take note and consider connecting them.",
        2 => "A vast cloud of interstellar gas and dust, forming unique and often beautiful cosmic formations. Nebulae can contain pockets of intense radiation, electromagnetic disturbances, or rare elements that make them valuable or dangerous to explore.",
        3 => "Black holes can distort space, emit powerful radiation, and possess an event horizon from which nothing can escape.",
        4 => "A planetary body that is not bound to any star and drifts through space. Rogue planets can have unique ecosystems, valuable resources, or mysterious origins",
        5 => "An area within a system with unusually high or low gravitational forces. Gravity wells can affect spacecraft trajectories, disrupt navigation, and require special maneuvers or equipment to safely traverse.",
        6 => "A localized distortion in gravitational forces within a system. Gravity anomalies can create unpredictable gravitational fields, gravitational anomalies, or gravity corridors that affect ship navigation, weapon accuracy, and even the passage of time.",
        7 => "A localized distortion in gravitational forces within a system. Gravity anomalies can create unpredictable gravitational fields, gravitational anomalies, or gravity corridors that affect ship navigation, weapon accuracy, and even the passage of time.",
        8 => "For some reason the barrier between here and one of the following planes is weaker here, and sometimes things 'slip through.' Roll 1D4 or pick: Void, Aether, Nether, Abyss",
        _ => throw new ArgumentOutOfRangeException()
    };
}