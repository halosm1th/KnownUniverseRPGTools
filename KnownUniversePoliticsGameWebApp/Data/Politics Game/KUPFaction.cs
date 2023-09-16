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

    public int FactionID { get; private set; }
    public string Name { get; }
    public FactionType FactionType { get; }
    public KUPPlayer? Player { get; private set; }
    public int Money { get; set; }
    public int Influence { get; private set; }
    public List<IKUPAsset> Assets { get; }

    public List<KUPCombatAsset> CombatAssets => Assets.OfType<KUPCombatAsset>().ToList();

    public KUPFaction(string name = "Empty Faction", int id =-10000, FactionType factionType = FactionType.Unclaimed, int money = 0, 
        int influence = 0, List<IKUPAsset> assets = default, KUPPlayer player = default)
    {
        Name = name;
        FactionType = factionType;
        Money = money;
        Influence = influence;

        FactionID = id;
        Player = player;
        if (player != null && (Player?.Faction == null || player?.Faction == default))
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

        foreach (var combat in Assets.OfType<KUPCombatAsset>())
        {
            combat.HasMoved = false;
        }
        
        UpdateFactionBasedOnMoneyAndInfluence();
    }

    private void UpdateFactionBasedOnMoneyAndInfluence()
    {
        if (Money > 0)
        {
            Influence = Influence + (Money / 5);
        }
        else if (Money < 0)
        {
            Influence = Influence - (Money / 2);
        }

        if (Influence > 0)
        {
            Money = Money + (Influence / 2);
        }
        else if (Influence < 0)
        {
            Money = Money + (Influence / 20);
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
        asset.Controller = new KUPFaction();
    }

    public void NewPlayer(KUPPlayer kupPlayer)
    {
        Player = kupPlayer;
    }

    public List<IKUPLocationAsset> GetFilledSystems()
    {
        return Assets.OfType<IKUPLocationAsset>().ToList();
    }

    public void DamageInfluence(int amountOfDamage)
    {
        Influence -= amountOfDamage;
    }

    public void DamageMoney(int amountOfDamage)
    {
        Money -= amountOfDamage;
    }

    public void DamageMilitary(int amountOfDamage)
    {
        var numbToHurt = amountOfDamage / 5;
        var milAssets = GetMilitaryAssets();
        var random = new Random(milAssets.Count + numbToHurt);
        for (int i = 0; i < numbToHurt; i++)
        {
            var asset = milAssets[random.Next(0,milAssets.Count)];
            KUPEventService.AddEventStatic(
                new KUPShipDamagedEvent(SenderID,asset.ReciverID,1));
        }
    }

    public List<KUPCombatAsset> GetMilitaryAssets()
    {
        return Assets.OfType<KUPCombatAsset>().ToList();
    }

    public void FactionExists()
    {
        if (Name == "Empty Faction")
        {
            FactionID = FactionID--;
            AddToEventService();
        }
    }
}