using Simple_Subsector_Generator;
using SixLabors.ImageSharp;
using TravellerMapSystem.Tools;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KnownUniversePoliticsGame : IKUPEventActor
{
    public string Name => "The Game";
    public static KUPEventService EventService;
    public int SenderID => 1919991701;
    public int ReciverID => 1919991701;
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }
    public KURPGSector Sector { get; } 
    
    public bool GameRunning { get; protected set; }
    public List<KUPFaction> Factions { get; }
    public List<KUPPlayer?> Players { get; }
    public int seed = 20001201;
    public static int CurrentAssetID = 0;

    public int CurrentRound = 1;
    public List<IKUPAsset> AssetsInPlay { get; }

    public KUPDrawSector KupDrawSector;
    public static Image _SectorImage = null;

    public Image SectorImage
    {
        get
        {
                return KupDrawSector.GenerateImage(false, Directory.GetCurrentDirectory() + "/Subsectors/KUPMap.png", false);
        }
    }

    public KnownUniversePoliticsGame()
    {
        AssetsInPlay = new List<IKUPAsset>();
        var thomas = new KUPPlayer("Thomas", "password", 100, 10001, 10001);
        var imp1 = new KUPPlayer("IMP1", "Empire", 10, 10002, 10002);
        var vrs1 = new KUPPlayer("VRS1", "Empire", 10, 10003, 10003);
        var ufe1 = new KUPPlayer("UFE1", "Federation", 10, 10004, 10004);
        var pirate = new KUPPlayer("PIRATE", "Federation", 10, 10005, 10005);
        var bank = new KUPPlayer("BANK", "Federation", 10, 10006, 10006);
        var food = new KUPPlayer("FOOD", "Federation", 10, 10007, 10007);
        Players = new List<KUPPlayer?>()
        {
            thomas, imp1, vrs1, ufe1
        };
        var generater = new KUPSectorGenerator("KUP Sector",true,seed,this,false);
        generater.Generate();
        Sector = generater.Sector;

        Factions = new List<KUPFaction>()
        {
            new ("Game Master", 0, FactionType.GM, 1000000,10000000,
                GetAssetsFromIDS(new ()
                {
                    1597
                }), thomas),
            new ("Bank",1,FactionType.Bank, 1000000,10000000,
                GetAssetsFromIDS(new ()
                {
                    1597
                }), bank),
            new ("Food",2,FactionType.Food, 1000000,10000000,
                GetAssetsFromIDS(new ()
                {
                }), food),
            new ("Pirates",3,FactionType.Pirates, 1000000,10000000,
                GetAssetsFromIDS(new ()
                {
                }), pirate),
            new KUPFaction("Test Imperials 3",4,FactionType.Imperial3,0,0,
                GetAssetsFromIDS(new ()
                {
                    0,1,5,7,8,9,10,12,13,16   
                }), imp1),
            new ("Test Versians",7, FactionType.Vers1,0,0,
                GetAssetsFromIDS(new ()
                {
                    1418, 1419, 1420, 1421, 1422, 1423, 1424, 1426, 1427, 1428, 1429, 1430,
                    1431, 1432, 1433, 1434, 1435, 1436, 1437, 1438, 1439 ,1440, 1441, 1442, 1443, 1444, 1445,
                    1448, 1450, 1447,954, 953, 959, 938, 939, 941, 940, 943,944, 945, 946, 947,
                    948, 949, 950, 951
                }), vrs1),
            new ("Test Fedeation", 10, FactionType.UFE1, 0,0,
                GetAssetsFromIDS(new ()
                {
                    1312,1315,1314,1316,1317,1319,1320, 1321, 1322,1323,1324,1325,1327,1328, 1326, 1329, 1330,
                    1840,1842, 1843, 1844, 1845
                }), ufe1)
            
        };
        KupDrawSector = new KUPDrawSector(Sector,Factions);

        AddToEventService();
    }

    public List<IKUPAsset?> GetAssetsFromIDS(List<int> assetIDs)
    {
        var assets = new List<IKUPAsset?>();

        foreach (var assetID in assetIDs)
        {
            assets.Add(AssetsInPlay.Find(x => x.assetID == assetID));
        }
        
        return assets;
    }

    public int GetNewAssetID()
    {
        return CurrentAssetID++;
    }

    
    public void NewAsset(IKUPAsset asset)
    {
        AssetsInPlay.Add(asset);
        
    }
    public void NewAsset(IKUPAsset asset, KUPFaction faction)
    {
        AssetsInPlay.Add(asset);
        asset.Controller = faction;
        faction.AddAsset(asset);
    }
    
    public void DestroyAsset(IKUPAsset asset, KUPFaction faction)
    {
        AssetsInPlay.Remove(asset);
        faction.DestroyAsset(asset);
    }

    public void EndOfTurn()
    {
        EventService.AddEvent(new IKUPMessageEvent(SenderID,-1,$"Round {CurrentRound} has ended. Come to the dining room."));
        foreach (var faction in Factions)
        {
            faction.Update();
        }

        CurrentRound++;
    }
    public void ProcessEvent(IKUPEvent evnt)
    {
        if (evnt.GetType() == typeof(IKUPMoneyTransferEvent))
        {
            MoneyTransfer(evnt as IKUPMoneyTransferEvent);
        }
    }

    private void MoneyTransfer(IKUPMoneyTransferEvent evnt)
    {
        var transferer = EventService.GetActorBySenderID(evnt.SenderID);
        var transferee = EventService.GetActorByReciverID(evnt.TargetAccountID);
        
        WithdrawMoney(transferer,evnt.AmountOfMoney);
        DespoitMoney(transferee, evnt.AmountOfMoney);
    }

    private void DespoitMoney(IKUPEventActor transferee, int evntAmountOfMoney)
    {
        if (transferee.GetType() == typeof(KUPPlayer))
        {
            ((KUPPlayer) transferee).PersonalFunds += evntAmountOfMoney;
        }
        
        if (transferee.GetType() == typeof(KUPFaction))
        {
            ((KUPFaction) transferee).Money += evntAmountOfMoney;
        }
    }

    private void WithdrawMoney(IKUPEventActor transferer, int evntAmountOfMoney)
    {
        if (transferer.GetType() == typeof(KUPPlayer))
        {
            ((KUPPlayer) transferer).PersonalFunds -= evntAmountOfMoney;
        }
        
        if (transferer.GetType() == typeof(KUPFaction))
        {
            ((KUPFaction) transferer).Money -= evntAmountOfMoney;
        }
    }

}