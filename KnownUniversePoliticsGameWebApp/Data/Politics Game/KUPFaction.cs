using System.Text;
using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

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

public enum FactionRelationshipOptions
{
    Peace,
    War,
    TotalAlliance,
    DefenceAlliance
}

public class KUPFaction : IKUPEventActor
{

    public int SenderID => FactionID;
    public int ReceiverID => FactionID;
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public Dictionary<KUPFaction, FactionRelationshipOptions> FactionRelationships;

    public int FactionID { get; private set; }
    public string Name { get; }
    public FactionType FactionType { get; }
    public KUPPlayer? Player { get; private set; }
    public int Money { get; set; }
    public int Influence { get; private set; }
    public List<IKUPAsset> Assets { get; }

    public List<KUPCombatAsset> CombatAssets => Assets.OfType<KUPCombatAsset>().ToList();
    public List<string> Summary { get; }
    public List<string> Goals { get; set; }
    public List<IKUPLocationAsset> LocationAssets => Assets.OfType<IKUPLocationAsset>().ToList();

    public KUPFaction(string name = "Empty Faction", int id =-10000, FactionType factionType = FactionType.Unclaimed, int money = 0, 
        int influence = 0, List<IKUPAsset?> assets = default, KUPPlayer player = default, 
        List<string> summary = default, List<string> goals = default)
    {
        Name = name;
        FactionType = factionType;
        Money = money;
        Influence = influence;
        Goals = goals;
        Summary = summary;

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
        FactionRelationships = new Dictionary<KUPFaction, FactionRelationshipOptions>();
    }

    public void AtWar(KUPFaction atWarWith)
    {
        FactionRelationships[atWarWith] = FactionRelationshipOptions.War;
    }

    public void Peace(KUPFaction atWarWith)
    {
        FactionRelationships[atWarWith] = FactionRelationshipOptions.Peace;
    }

    public void Alliance(KUPFaction atWarWith)
    {
        FactionRelationships[atWarWith] = FactionRelationshipOptions.TotalAlliance;
    }

    public void Defense(KUPFaction atWarWith)
    {
        FactionRelationships[atWarWith] = FactionRelationshipOptions.DefenceAlliance;
    }
    
    public void SetStartingFactionRelationships(List<KUPFaction> factions)
    {
        foreach(var faction in factions){
            NewFaction(faction,FactionRelationshipOptions.Peace);
        }
    }

    public void NewFaction(KUPFaction newFaction, FactionRelationshipOptions relationship)
    {
        FactionRelationships.Add(newFaction,relationship);
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
            if (Player.Faction.FactionType == FactionType.Pirates)
            {
                if (asset is not KUPCombatAsset)
                {
                    Money +=  (-1 * asset.MoneyTotal);
                    Influence += (-1 * asset.MoneyTotal);
                }
            }

            else
            {
                Money += asset.MoneyTotal;
                Influence += asset.MoneyTotal;
            }
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
        if (!Assets.Contains(asset))
        {
            Assets.Add(asset);
            asset.Controller = this;
        }
    }

    public void DestroyAsset(IKUPAsset asset)
    {
        if (Assets.Contains(asset))
        {
            Assets.Remove(asset);
            asset.Controller = KnownUniversePoliticsGame.GameMaster;
        }
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
        if (milAssets.Count > 0)
        {
            var random = new Random(milAssets.Count + numbToHurt);
            for (int i = 0; i < numbToHurt; i++)
            {
                var asset = milAssets[random.Next(0, milAssets.Count)];
                KUPEventService.AddEventStatic(
                    new KUPShipDamagedEvent(SenderID, asset.ReceiverID, 1));
            }
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