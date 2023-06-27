namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPOtherAsset : IKUPLocationAsset
{
    public int MoneyTotal => MoneyIncome - UpKeepCost;
    public int InfluenceTotal => MoralIncome - MoralCost;
    public int MoneyIncome => GetIncome();

    private int GetIncome()
    {
        var income = 0;
        foreach (var tradecode in Other.GetTradeCodes())
        {
            if (tradecode == KURPGTradeCodes.Ht) income += 25;
            else if (tradecode == KURPGTradeCodes.Ht) income += 50;
            else if (tradecode == KURPGTradeCodes.Ha) income += 25;
            else if (tradecode == KURPGTradeCodes.Ag) income += 25;
            else if (tradecode == KURPGTradeCodes.Ma) income += 50;
            else if (tradecode == KURPGTradeCodes.Ec) income += 100;
            else if (tradecode == KURPGTradeCodes.Re) income += 25;
            else if (tradecode == KURPGTradeCodes.Th) income += 500;
            else if (tradecode == KURPGTradeCodes.Sc) income += 25;
            else if (tradecode == KURPGTradeCodes.Rf) income += 25;
        }

        return income;
    }

    public int UpKeepCost => GetMoneyUpkeep();

    private int GetMoneyUpkeep()
    {
        var costs = 0;
        foreach (var tradecode in Other.GetTradeCodes())
        {
            if (tradecode == KURPGTradeCodes.Is) costs -= 10;
            else if (tradecode == KURPGTradeCodes.Io) costs += 25;
            else if (tradecode == KURPGTradeCodes.Bh) costs += 50;
            else if (tradecode == KURPGTradeCodes.Ag) costs += 10;
            else if (tradecode == KURPGTradeCodes.Hp) costs += 10;
            else if (tradecode == KURPGTradeCodes.Lp) costs += 25;
            else if (tradecode == KURPGTradeCodes.Rs) costs += 10;
            else if (tradecode == KURPGTradeCodes.Rl) costs += 10;
            else if (tradecode == KURPGTradeCodes.Gw) costs += 50;
            else if (tradecode == KURPGTradeCodes.Lt) costs += 10;

        }

        return costs;
    }

    public int MoralIncome => GetMoral();

    private int GetMoral()
    {
        var moral = 0;
        foreach (var tradecode in Other.GetTradeCodes())
        {
            if (tradecode == KURPGTradeCodes.Hp) moral -= 10;
            else if (tradecode == KURPGTradeCodes.Hl) moral += 10;
            else if (tradecode == KURPGTradeCodes.Ha) moral += 10;
            else if (tradecode == KURPGTradeCodes.Gw) moral += 50;
            else if (tradecode == KURPGTradeCodes.Rl) moral += 25;
            else if (tradecode == KURPGTradeCodes.Rs) moral += 25;
            else if (tradecode == KURPGTradeCodes.Sp) moral += 10;
            else if (tradecode == KURPGTradeCodes.Sc) moral += 10;

        }

        return moral;
    }

    public int MoralCost => GetMoralCosts();

    private int GetMoralCosts()
    {
        var moral = 0;
        foreach (var tradecode in Other.GetTradeCodes())
        {
            if (tradecode == KURPGTradeCodes.Lp) moral -= 10;
            else if (tradecode == KURPGTradeCodes.Ll) moral += 10;
            else if (tradecode == KURPGTradeCodes.Is) moral += 10;
            else if (tradecode == KURPGTradeCodes.Io) moral += 25;
            else if (tradecode == KURPGTradeCodes.Bh) moral += 50;

        }

        return moral;
    }

    public int assetID { get; }
    public string Name { get; }
    public KUPLocation Location { get; }
    public KUPFaction? Controller { get; set; }
    public KupPointsOfInterestOther Other { get; }
    public KUPPointsOfInterest POI => Other;

    public KUPOtherAsset(KupPointsOfInterestOther other, int id)
    {
        Other = other;
        Location = new KUPLocation(other.InSystem.DisplayX,other.InSystem.DisplayY);
        Name = other.SubtypeName + " in " + other.InSystem.Name;
        assetID = id;
    }
    public override string ToString()
    {
        return $"#{assetID} {Name} ({Location}) [{Controller?.Name ?? "No Controller"}] ${MoneyIncome-UpKeepCost} 😊{MoralIncome-MoralCost}.";
    }
}