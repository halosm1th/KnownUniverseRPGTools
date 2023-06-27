namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPAsset
{
    int MoneyTotal { get; }
    int InfluenceTotal { get; }
    int MoneyIncome { get; }
    int UpKeepCost { get; }
    int MoralIncome { get; }
    int MoralCost { get; }
    int assetID { get; }
    string Name { get; }
    KUPLocation Location { get; }
    KUPFaction Controller { get; set; }
}

public enum CombatAssetSize
{
    Small,
    Medium,
    Large, 
    Station
}