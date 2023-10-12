namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPPOIAsset
{
    public int MoneyTotal { get; }
    public int InfluenceTotal { get; }
    public int MoneyIncome { get; }
    public int UpKeepCost { get; }
    public  int MoralIncome { get; }
    public int MoralCost { get; }
    public int assetID { get; }
    public  string Name { get; }
    public   KUPLocation Location { get; }
    public  KUPFaction Controller { get; set; }
    public List<KURPGTradeCodes> TradeCodes { get; }
    public KUPPointsOfInterest PointOfInterst { get; }
}