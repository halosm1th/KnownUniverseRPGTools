using System.Text;

namespace KnownUniversePoliticsGameWebApp.Data;

public enum FactionType
{
    Empty,
    Unclaimed,
    Bank,
    Food,
    Pirates,
    GM,
    Imperial1,
    Imperial2,
    Imperial3,
    XiaoMing1,
    XiaoMing2,
    XiaoMing3,
    Vers1,
    Vers2,
    Vers3,
    UFE1,
    UFE2,
    UFE3,
    Deutchria1,
    Deutchri2,
    Deutchria3,
}

public class KUPFaction : IKUPEventActor
{

    public int SenderID => FactionID;
    public int ReciverID => FactionID;
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public int FactionID { get; }
    public string Name { get; }
    public FactionType FactionType { get; }
    public KUPPlayer? Player { get; private set; }
    public int Money { get; set; }
    public int Influence { get; private set; }
    public List<IKUPAsset> Assets { get; }

    public KUPFaction(string name = "Empty Faction", int id =-1, FactionType factionType = FactionType.Unclaimed, int money = 0, 
        int influence = 0, List<IKUPAsset> assets = default, KUPPlayer player = default)
    {
        Name = name;
        FactionType = factionType;
        Money = money;
        Influence = influence;

        FactionID = id;
        Player = player;
        if (Player?.Faction == null || player?.Faction == default)
        {
            Player.Faction = this;
        }

        if (assets == default)
        {
            assets = new List<IKUPAsset>();
        }
        Assets = assets;
        foreach (var asset in Assets)
        {
            asset.Controller = this;
        }

        AddToEventService();
    }

    public override string ToString()
    {
        return $"{Name} ({Player?.Name ?? "No Controller"}) ${Money} 😊{Influence}."; //+ GetAssetList();
    }

    public string GetAssetList()
    {
        var assetList = new StringBuilder();

        foreach (var asst in Assets)
        {
            assetList.Append($"\n\t{asst.Name} ({asst.Location}) ${asst.MoneyTotal} 😊{asst.InfluenceTotal}");
        }
        
        return assetList.ToString();
    }

    public void Update()
    {
        foreach (var asset in Assets)
        {
            Money += asset.MoneyTotal;
            Influence += asset.MoneyTotal;
        }
    }

    public void AddAsset(IKUPAsset asset)
    {
        Assets.Add(asset);
        asset.Controller = this;
    }

    public void DestroyAsset(IKUPAsset asset)
    {
        Assets.Remove(asset);
    }

    public void NewPlayer(KUPPlayer kupPlayer)
    {
        Player = kupPlayer;
    }
}