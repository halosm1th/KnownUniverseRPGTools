namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPSystemAsset : IKUPLocationAsset
{
    public List<IKUPPOIAsset> POIAssets { get; }
    public int MoneyTotal => POIAssets.Aggregate(0, (h, t) => h + t.MoneyTotal);
    public int InfluenceTotal => POIAssets.Aggregate(0, (h, t) => h + t.InfluenceTotal);
    public int MoneyIncome => POIAssets.Aggregate(0, (h, t) => h + t.MoneyIncome);
    public int UpKeepCost => POIAssets.Aggregate(0, (h, t) => h + t.UpKeepCost);
    public int MoralIncome => POIAssets.Aggregate(0, (h, t) => h + t.MoralIncome);
    public int MoralCost => POIAssets.Aggregate(0, (h, t) => h + t.MoralCost);
    public int assetID { get; }
    public string Name { get; }
    public KUPLocation Location { get; }
    public KUPFaction Controller { get; set; }
    public KUPPointsOfInterest POI => POIAssets.OfType<KupPointsOfInterestStation>().First();

    public List<KURPGTradeCodes> TradeCodes =>
        POIAssets
            .Aggregate(new List<KURPGTradeCodes>(), (h, t)
                =>
            {

                h.AddRange(t.TradeCodes);
                return h;
            });

    public KUPSystemAsset(List<IKUPPOIAsset> poiAssets, int assetId, string name, KUPLocation location, KUPFaction controller)
    {
        POIAssets = poiAssets;
        assetID = assetId;
        Name = name;
        Location = location;
        Controller = controller;
    }
}