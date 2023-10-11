using KnownUniversePoliticsGameWebApp.Data.Subsector_Generator.Regions_of_Space;
using Microsoft.AspNetCore.Components.Web;
using KUP_Simple_Sector_Generator;
using SixLabors.ImageSharp;
using TravellerMapSystem.Tools;

namespace KnownUniversePoliticsGameWebApp.Data.Politics_Game;

public class KnownUniversePoliticsGame : IKUPEventActor
{
    #region Variables

    public string Name => "The Game";
    public static KUPEventService EventService;
    public static KUPFaction GameMaster;

    public int SenderID => 1919991701;
    public int ReceiverID => 1919991701;
    private int _shipIDs = 600000;

    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public KUPSector? Sector { get; }

    public bool GameRunning { get; protected set; }
    public List<KUPFaction> Factions { get; }
    public List<KUPPlayer?> Players { get; }
    private int seed = 20001201;
    private static int CurrentAssetId = 0;

    private int CurrentRound = 1;
    public List<IKUPAsset> AssetsInPlay { get; } = new List<IKUPAsset>();
    public List<KUPCombatAsset> AssetsToFight { get; set; }

    private readonly KUPDrawSector KupDrawSector;

    public Image SectorImage
    {
        get
        {
            return KupDrawSector.GenerateImage(false, Directory.GetCurrentDirectory() + "/Subsectors/KUPMap.png",
                false);
        }
    }

    #endregion

    #region Constructor

    public KnownUniversePoliticsGame()
    {
        AddToEventService();
        //Setup assets
        AssetsInPlay = new List<IKUPAsset>();

        //Setup Playesr
        var thomas = new KUPPlayer("Thomas", "password", 100, 10001, 10001);
        var imp1 = new KUPPlayer("IMP1", "Empire", 10, 10002, 10002);
        var vrs1 = new KUPPlayer("VRS1", "Empire", 10, 10003, 10003);
        var ufe1 = new KUPPlayer("UFE1", "Federation", 10, 10004, 10004);
        var pirate = new KUPPlayer("PIRATE", "Federation", 10, 10005, 10005);
        var bank = new KUPPlayer("BANK", "Federation", 10, 10006, 10006);
        var food = new KUPPlayer("FOOD", "Federation", 10, 10007, 10007);
        Players = new List<KUPPlayer?>()
        {
            thomas, imp1, vrs1, ufe1,pirate,bank,food
        };

        //Setup the subsector
        var generator = new KUPSectorGenerator("KUP Sector", true, seed, this, false);
        generator.Generate();
        Sector = generator.Sector;
        //Set the politics game for location so it can do name lookups.
        KUPLocation.politicsGame = this;

        //the special faction used for handling a lot of game stuff is the game master faction
        GameMaster = new("Game Master", 0, FactionType.GM, 1000000, 10000000,
            GetAssetsFromIDS(new()
            {
                1597
            }), thomas);

        //Generate the factions for the players.
        Factions = new List<KUPFaction>()
        {
            GameMaster,
            new("Bank", 1, FactionType.Bank, 1000000, 10000000,
                GetAssetsFromIDS(new()
                {
                    1597
                }), bank),
            new("Food", 2, FactionType.Food, 1000000, 10000000,
                GetAssetsFromIDS(new()
                {
                }), food),
            new("Pirates", 3, FactionType.Pirates, 0, 0,
                GetAssetsFromIDS(new()
                {
                    38, 40, 44, 498, 499, 965, 968, 52, 502,504,979,74,78,
                    525, 996, 1509, 528, 1006,1008, 1012,94,26,1026,97,109,110,567,1037,
                    1040,1553, 166,172, 1095, 1611, 178, 1622,1110, 1114,1624,202, 1117,
                    1629, 642, 1124, 1125,1637,215,224,226 ,227,656,1143,1653,1658 ,232,
                    659, 661,1666 ,1667, 238,667,1164,1166,1169, 1674, 1212,1213,1737,735,
                    1742, 1750,304,751,750,1756,1761,312,1767, 316, 762,1243,771,773,1248,1774,
                    1781,1782,332,787,1256,1258, 1259, 1262,1265,788,789,1268,1269,1273,343,807,
                    1281, 1288, 1805, 1809, 391,395,396,858,1854,399,866,409,871,413,878,1872,1375,
                    1879, 1887, 423,1380, 1888,893,899,1893,1391,1902, 915,1397,1404,1905,924,929,
                    1409,1913,1915, 1917 
                }), pirate),
            new KUPFaction("Test Imperials 3", 4, FactionType.Imperial3, 0, 0,
                GetAssetsFromIDS(new()
                {
                    0, 1, 5, 7, 8, 9, 10, 12, 13, 16,
                    51, 54, 55, 56, 57, 58, 75, 2, 41, 43, 3, 45,
                    4, 48, 50
                }), imp1),
            new("Test Versians", 7, FactionType.Vers1, 0, 0,
                GetAssetsFromIDS(new()
                {
                    1418, 1419, 1420, 1421, 1422, 1423, 1424, 1426, 1427, 1428, 1429, 1430,
                    1431, 1432, 1433, 1434, 1435, 1436, 1437, 1438, 1439, 1440, 1441, 1442, 1443, 1444, 1445,
                    1448, 1450, 1447, 954, 953, 959, 938, 939, 941, 940, 943, 944, 945, 946, 947,
                    948, 949, 950, 951
                }), vrs1),
            new("Test Fedeation", 10, FactionType.UFE1, 0, 0,
                GetAssetsFromIDS(new()
                {
                    1312, 1315, 1314, 1316, 1317, 1319, 1320, 1321, 1322, 1323, 1324, 1325, 1327, 1328, 1326, 1329,
                    1330,
                    1840, 1842, 1843, 1844, 1845
                }), ufe1)
        };

        SetupBaseRelationships();

        NewAsset(new KUPCombatAsset(
            new KUPLocation(3, 3), Factions.First(x => x.FactionID == 4), CombatAssetSize.Large,
            _shipIDs++, 80081222), Factions.First(x => x.FactionID == 4));
        KupDrawSector = new KUPDrawSector(Sector, Factions, this);


        var totalIncome = AssetsInPlay.Aggregate(0, (h, t) => h + t.MoneyTotal);
        var totalInfluence = AssetsInPlay.Aggregate(0, (h, t) => h + t.InfluenceTotal);
        Console.WriteLine($"Total income: {totalIncome}. Total influence: {totalInfluence}");

        AssetsToFight = new List<KUPCombatAsset>();
    }

    private void SetupBaseRelationships()
    {
        foreach (var faction in Factions)
        {
            faction.SetStartingFactionRelationships(Factions
                .Where(x => x.FactionID != faction.FactionID).ToList());
        }
    }

    public string GetLocationName(int x, int y)
    {
        return Sector.GetSystemName(x, y);
    }

    #endregion

    #region Assets

    public IKUPAsset? GetAssetFromID(int assetID)
    {
        return AssetsInPlay.Find(x => x.assetID == assetID);
    }
    
    public List<IKUPAsset?> GetAssetsFromIDS(List<int> assetIDs)
    {
        var assets = new List<IKUPAsset?>();

        foreach (var assetID in assetIDs)
        {
            assets.Add(GetAssetFromID(assetID));
        }

        return assets;
    }

    public int GetNewAssetID()
    {
        return CurrentAssetId++;
    }


    public void NewAsset(IKUPAsset asset)
    {
        if (asset.Controller != null) NewAsset(asset, asset.Controller);
        else AssetsInPlay.Add(asset);
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

    #endregion

    public void EndOfTurn()
    {
        EventService.AddEvent(new IKUPMessageEvent(SenderID, -1,
            $"Round {CurrentRound} has ended. Come to the dining room."));
        foreach (var faction in Factions)
        {
            faction.Update();
        }

        HandleCombats();

        RemoveDestroyedAssets();

        CurrentRound++;
    }

    private List<KUPCombatAsset> CombatAssets => AssetsInPlay.OfType<KUPCombatAsset>().ToList();
    
    private void HandleCombats()
    {
        foreach(var asset in AssetsToFight){
            if (asset.HP > 0)
            {
                var targetShip = CombatAssets
                    .First(x => x.Location == asset.Location
                                && x.AtWar(asset)
                                && x.HP > 0);
                
                EventService.AddEvent(
                    new KUPShipDamagedEvent(asset.SenderID, targetShip.ReceiverID,asset.AttackPower));
            }
        }
        
        AssetsToFight = new List<KUPCombatAsset>();
    }

    private void RemoveDestroyedAssets()
    {
        var toBeDestroyed = new List<KUPCombatAsset>();
        foreach (var asset in AssetsInPlay.Where(x => x.GetType() == typeof(KUPCombatAsset)))
        {
            if (((KUPCombatAsset) asset).HP <= 0)
            {
                toBeDestroyed.Add(asset as KUPCombatAsset);
            }
        }

        foreach (var ass in toBeDestroyed)
        {
            DestroyAsset(ass, ass.Controller);
        }
    }

    #region Events

    public void ProcessEvent(IKUPEvent evnt)
    {
        evnt.RunEvent(this,EventService);
    }

    public void BuildShip(KUPLocation buildLoc, KUPFaction buildFaction, CombatAssetSize Size)
    {
        NewAsset(new KUPCombatAsset(buildLoc, buildFaction, Size, GetNewAssetID(), _shipIDs++),
            buildFaction);
    }

    public int GetOperationCost(KUPOPerationSize evntOperationSize, KUPOperationType evntOperationNumber)
    {
        var amount = evntOperationSize switch
        {
            KUPOPerationSize.Small => 10,
            KUPOPerationSize.Medium => 25,
            KUPOPerationSize.Large => 50,
            _ => 50,
        };

        amount *= evntOperationNumber switch
        {
            KUPOperationType.InfluenceAttack => 2,
            KUPOperationType.MoneyAttack => 3,
            KUPOperationType.MilitaryAttack => 5,
            _ => 2,
        };

        return amount;
    }

    public int GetAmountOfOperationDamage(KUPOPerationSize evntOperationSize)
        => evntOperationSize switch
        {
            KUPOPerationSize.Small => 25,
            KUPOPerationSize.Medium => 75,
            KUPOPerationSize.Large => 125,
            _ => 250,
        };

    public void DepositMoney(IKUPEventActor transferee, int evntAmountOfMoney)
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

    public void WithdrawMoney(IKUPEventActor transferer, int evntAmountOfMoney)
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

    #endregion

    public KUPFaction GetFaction(string name)
    {
        return Factions.First(x => x.Name == name);
    }

    public KUPFaction GetFaction(int id)
    {
        return Factions.First(x => x.FactionID == id);
    }


    public bool CouldCaptureSystem(KUPFilledSystem system, KUPFaction who)
    {
        if (system.SystemsPrimaryStation?.PrimaryStationAsset.Controller == null ||
            system.SystemsPrimaryStation.PrimaryStationAsset.Controller.FactionType == FactionType.Unclaimed ||
            system.SystemsPrimaryStation.PrimaryStationAsset.Controller == who) return false;
        var locX = system.DisplayX;
        var locY = system.DisplayY;
        var ships = AssetsInPlay.Where(x => x.Location.SystemX == locX && x.Location.SystemY == locY);
        return ships.Any(x => x.Controller == system.SystemsPrimaryStation.PrimaryStationAsset.Controller);

    }

    public void AdminTranferAssets(KUPFaction targetFaction, List<IKUPAsset> assetsToTransfer)
    {
        //First transfer all the selects assets to the GM from their respective owners.
        foreach (var asset in assetsToTransfer)
        {
            EventService.AddEvent(new KUPAssetTransferEvent(asset.Controller.SenderID,
                GameMaster.ReceiverID,new List<int>(){asset.assetID}));
        }
        
        //Then transfer the assets from the GM to the target player.
            EventService.AddEvent(
                new KUPAssetTransferEvent(GameMaster.SenderID,targetFaction.FactionID,
                    assetsToTransfer.Select(x => x.assetID).ToList()));
        
    }

    public void SetPlayerFaction(string name, int factionId)
    {
        var player = Players.First(x => x.Name == name);
        var faction = GetFaction(factionId);
        
        EventService.AddEvent(
            new KUPPlayerChangeFactionEvent(player.SenderID,faction.ReceiverID));
    }

    public List<IKUPLocationAsset> GetBuildLocations(KUPFaction? faction)
    {
        var locations = new List<IKUPLocationAsset>();
        if (faction != null)
            foreach (var location in faction.Assets.OfType<IKUPLocationAsset>())
            {
                var tradeCodes = location.POI.GetTradeCodes();
                if (faction.FactionType == FactionType.Pirates)
                {
                    if (tradeCodes.Contains(KURPGTradeCodes.Bh)
                        || tradeCodes.Contains(KURPGTradeCodes.Is)
                        || tradeCodes.Contains(KURPGTradeCodes.Io))
                    {
                        locations.Add(location);
                    }
                }
                else
                {
                    if (tradeCodes.Contains(KURPGTradeCodes.Ms)
                        || tradeCodes.Contains(KURPGTradeCodes.Mb))
                    {
                        locations.Add(location);
                    }
                }
            }

        return locations;
    }

    public void CreateNewFaction(string name, int facID, FactionType factionType,
        int money, int influence, List<IKUPAsset> assets, KUPPlayer? player, List<string> summary,
        List<string> goals)
    {
        var faction = new KUPFaction(name,facID,factionType,money,influence,assets,player,summary,goals);
        Factions.Add(faction);
        
        var oldFac = player.ChangeFaction(faction);       
        oldFac?.NewPlayer(null);
        //Put the assigned player in charge of the new faction and assign the old faction to no one.

    }
}