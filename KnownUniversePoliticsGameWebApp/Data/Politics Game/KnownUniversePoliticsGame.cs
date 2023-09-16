using System.Runtime.CompilerServices;
using KnownUniversePoliticsGameWebApp.Pages.Military_Pages;
using Microsoft.AspNetCore.Mvc;
using Simple_Subsector_Generator;
using SixLabors.ImageSharp;
using TravellerMapSystem.Tools;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KnownUniversePoliticsGame : IKUPEventActor
{
    #region Variables
    public string Name => "The Game";
    public static KUPEventService EventService;
    public int SenderID => 1919991701;
    public int ReciverID => 1919991701;
    private int ShipIDs = 600000;
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
    #endregion
    #region Constructor

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
                GetAssetsFromIDS(new()
                {
                    0, 1, 5, 7, 8, 9, 10, 12, 13, 16,
                    51, 54, 55, 56, 57, 58, 75, 2, 41, 43, 3, 45,
                    4, 48, 50
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
        
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(3,3),Factions.First(x => x.FactionID == 4),CombatAssetSize.Large,
            ShipIDs++,80081222), Factions.First(x => x.FactionID == 4));
        KupDrawSector = new KUPDrawSector(Sector,Factions, this);

        AddToEventService();

        var totalIncome = AssetsInPlay.Aggregate(0, (h, t) => h + t.MoneyTotal);
        var totalInfluence = AssetsInPlay.Aggregate(0, (h, t) => h + t.InfluenceTotal);
        Console.WriteLine($"Total income: {totalIncome}. Total influence: {totalInfluence}");
    }
#endregion
    #region Assets
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
        if(asset.Controller != null) NewAsset(asset,asset.Controller);
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
        EventService.AddEvent(new IKUPMessageEvent(SenderID,-1,$"Round {CurrentRound} has ended. Come to the dining room."));
        foreach (var faction in Factions)
        {
            faction.Update();
        }
        
        RemoveDestroyedAssets();
        
        CurrentRound++;
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
            
            DestroyAsset(ass,ass.Controller);
        }
    }

    #region Events
    public void ProcessEvent(IKUPEvent evnt)
    {
        if (evnt.GetType() == typeof(IKUPMoneyTransferEvent))
        {
            MoneyTransfer(evnt as IKUPMoneyTransferEvent);
        }
        else if (evnt.GetType() == typeof(KUPMoneyWithdrawEvent))
        {
            MoneyWithdraw(evnt as KUPMoneyWithdrawEvent);
        }
        else if (evnt.GetType() == typeof(KUPMoneyDepositEvent))
        {
            MoneyDeposit(evnt as KUPMoneyDepositEvent);
        }else if (evnt.GetType() == typeof(KUPOperationEvent))
        {
            Operation(evnt as KUPOperationEvent);
        }else if (evnt.GetType() == typeof(KUPShipDamagedEvent))
        {
            ShipDamage(evnt as KUPShipDamagedEvent);
        }else if (evnt.GetType() == typeof(KUPBuildShipEvent))
        {
            BuildShip(evnt as KUPBuildShipEvent);
        }else if (evnt.GetType() == typeof(KUPMoveAssetEvent))
        {
            MoveAsset(evnt as KUPMoveAssetEvent);
        }else if (evnt.GetType() == typeof(KUPTakeAssetEvent))
        {
            TakeAsset(evnt as KUPTakeAssetEvent);
        }else if (evnt.GetType() == typeof(KUPTakeSystemEvent))
        {
            TakeSystem(evnt as KUPTakeSystemEvent);
        }else if (evnt.GetType() == typeof(KUPCaptureSystemEvent))
        {
            CaptureSystem(evnt as KUPCaptureSystemEvent);
        }else if (evnt.GetType() == typeof(KUPBuyStoreEvent))
        {
            StoreBuy(evnt as KUPBuyStoreEvent);
        }else if (evnt.GetType() == typeof(KUPAssetTransferEvent))
        {
            TransferAsset(evnt as KUPAssetTransferEvent);
        }
    }

    private void TransferAsset(KUPAssetTransferEvent evnt)
    {
        var sender = Factions.First(x => x.SenderID == evnt.SenderID);
        var reciver = Factions.First(x => evnt.TargetFactionReciverID == x.ReciverID);
        var targetAssetIDs = evnt.AssetsToTransfer;
        var assetsToTransfer = new List<IKUPAsset>();
        
        foreach (var asset in sender.Assets)
        {
            if (targetAssetIDs.Contains(asset.assetID))
            {
                assetsToTransfer.Add(asset);
            }
        }
        
        foreach (var asset in assetsToTransfer)
        {
            sender.DestroyAsset(asset);
            reciver.AddAsset(asset);
        }

        EventService.AddEvent(new IKUPMessageEvent(evnt.SenderID,evnt.TargetFactionReciverID,$"{sender.Name} " +
            $"has given you the following assets w/ IDs: {targetAssetIDs.Aggregate("", (h,t) => h + ", " + t)}"));
        
        EventService.AddEvent(new IKUPMessageEvent(evnt.TargetFactionReciverID,evnt.SenderID,$"{reciver.Name} " +
            $"has received the assets you gave them  w/ the following IDs: {targetAssetIDs.Aggregate("", (h,t) => h + ", " + t)}"));

    }

    private void StoreBuy(KUPBuyStoreEvent evnt)
    {
        var sender = Players.First(x => x.SenderID == evnt.SenderID);
        sender.PersonalFunds= sender.PersonalFunds - evnt.Cost;
        EventService.AddEvent(
            new KUPStoreSomeoneBought(sender.SenderID,
                GetFaction("Food").ReciverID,sender.Name,
                evnt.ItemToBuy,evnt.Amount));
    }

    private void CaptureSystem(KUPCaptureSystemEvent evnt)
    {
        var taker = EventService.GetActorBySenderID(evnt.SenderID);
        var targetStation = AssetsInPlay.First(x => x.assetID == evnt.SystemStationID);
        var previousHolder = targetStation.Controller;
        previousHolder.DestroyAsset(targetStation);
        
        var takeFact = Factions.First(x => x == taker);
        takeFact.AddAsset(targetStation);
        
        EventService.AddEvent(new IKUPMessageEvent(
            taker.SenderID,previousHolder.ReciverID,$"You have lost control of {targetStation.Location} to {taker.Name}"));
    }

    private void TakeSystem(KUPTakeSystemEvent evnt)
    {
        var taker = EventService.GetActorBySenderID(evnt.SenderID);
        var targetStation = AssetsInPlay.First(x => x.assetID == evnt.SystemStationID);
        var takeFact = Factions.First(x => x == taker);
        takeFact.AddAsset(targetStation);
    }

    private void TakeAsset(KUPTakeAssetEvent evnt)
    {
        var taker = EventService.GetActorBySenderID(evnt.SenderID);
        var targetStation = AssetsInPlay.First(x => x.assetID == evnt.AssetID);
        var takeFact = Factions.First(x => x == taker);
        takeFact.AddAsset(targetStation);
    }

    private void MoveAsset(KUPMoveAssetEvent evnt)
    {
        var target = (EventService.GetActorByReciverID(evnt.TargetID) as KUPCombatAsset);
        if (!target.ChangeLocationTo(evnt.Destination))
        {
            EventService.AddEvent(new IKUPMessageEvent(SenderID,evnt.SenderID,"Error building your ship. The location was invalid."));
        }
    }

    private void BuildShip(KUPBuildShipEvent evnt)
    {
        var cost = KUPCombatAsset.GetCosts(evnt.Size);
        var buildLoc = AssetsInPlay.First(x => x.assetID == evnt.BuildLocationID).Location;
        var buildFaction = Factions.First(x => x.SenderID == evnt.SenderID);

        if (buildFaction.Money >= cost)
        {
            buildFaction.Money = buildFaction.Money - cost;
            NewAsset(new KUPCombatAsset(buildLoc, buildFaction, evnt.Size, GetNewAssetID(), ShipIDs++),
                buildFaction);
        }
    }
    
    private void ShipDamage(KUPShipDamagedEvent evnt)
    {
        var ship = AssetsInPlay.OfType<KUPCombatAsset>()
            .First(x => x.ReciverID == evnt.TargetID);
        if (ship.HP - evnt.AmountOfDamage <= 0)
        {
            EventService.AddEvent(
                new IKUPMessageEvent(evnt.SenderID,
                    evnt.TargetID, $"Ship was Destroyed."));
            
            EventService.AddEvent(
                new IKUPMessageEvent(evnt.TargetID,
                    evnt.SenderID, $"Ship was Destroyed."));
            DestroyAsset(ship, ship.Controller);
        }
        else
        {
            ship.DoDamage(evnt.AmountOfDamage);
            EventService.AddEvent(
                new IKUPMessageEvent(evnt.SenderID,
                    evnt.TargetID, $"Took 1 point of damage."));
            EventService.AddEvent(
                new IKUPMessageEvent(evnt.TargetID,
                    evnt.SenderID, $"Took 1 point of damage."));
        }
    }


    private void Operation(KUPOperationEvent evnt)
    {
        var amountOfDamage = GetAmountOfOperationDamage(evnt.OperationSize);
        var operationCost = GetOperationCost(evnt.OperationSize, evnt.OperationNumber);
        var target = Factions.First(x => x.FactionID == evnt.OperationTarget);

        ApplyCost(EventService.GetActorBySenderID(evnt.SenderID), operationCost, evnt.OperationNumber);
        ApplyDamage(target, amountOfDamage, evnt.OperationNumber);
        
        EventService.AddEvent(
            new IKUPMessageEvent(evnt.SenderID,
                evnt.OperationTarget, evnt.OperationMessage));
    }

    private void ApplyCost(IKUPEventActor sender, int operationCost, KUPOperationType evntOperationNumber)
    {
        var faction = GetFaction(sender.Name);
        if (evntOperationNumber == KUPOperationType.InfluenceAttack)
        {
            faction.DamageInfluence(operationCost);
        }else if (evntOperationNumber == KUPOperationType.MoneyAttack)
        {
            faction.DamageMoney(operationCost);
        }else if (evntOperationNumber == KUPOperationType.MilitaryAttack)
        {
            faction.DamageInfluence(operationCost);
            faction.DamageMoney(operationCost);
        }
    }

    private void ApplyDamage(IKUPEventActor effectedPerson, int amountOfDamage, KUPOperationType type)
    {
        var faction = Factions.First(x =>
            x.Name == effectedPerson.Name);
        if (type == KUPOperationType.InfluenceAttack)
        {
            faction.DamageInfluence(amountOfDamage);
        }else if (type == KUPOperationType.MoneyAttack)
        {
            
            faction.DamageMoney(amountOfDamage);
        }else if (type == KUPOperationType.MilitaryAttack)
        {
            faction.DamageMilitary(amountOfDamage);
        }
    }

    public int GetOperationCost(KUPOPerationSize evntOperationSize, KUPOperationType evntOperationNumber)
    {
        var amount = evntOperationSize switch
        {
            KUPOPerationSize.Small => 10,
            KUPOPerationSize.Medium => 25,
            KUPOPerationSize.Large => 50
        };

        amount *= evntOperationNumber switch
        {
            KUPOperationType.InfluenceAttack => 2,
            KUPOperationType.MoneyAttack => 3,
            KUPOperationType.MilitaryAttack => 5
        };

        return amount;
    }

    private int GetAmountOfOperationDamage(KUPOPerationSize evntOperationSize)
        => evntOperationSize switch
        {
            KUPOPerationSize.Small => 25,
            KUPOPerationSize.Medium => 75,
            KUPOPerationSize.Large => 125
        };

    private void MoneyDeposit(KUPMoneyDepositEvent evnt)
    {
        var transferee = EventService.GetActorByReciverID(evnt.TargetAccountID);
        var transferer = Factions.First(x => x.Name == "Bank");
        EventService.AddEvent(new IKUPMessageEvent(evnt.SenderID,transferee.ReciverID,$"{evnt.AmountOfMoney}"));
        
        WithdrawMoney(transferer,evnt.AmountOfMoney);
        DespoitMoney(transferee, evnt.AmountOfMoney);
    }
    
    private void MoneyWithdraw(KUPMoneyWithdrawEvent evnt)
    {
        var transferer = EventService.GetActorBySenderID(evnt.SenderID);
        var transferee = Factions.First(x => x.Name == "Bank");
        EventService.AddEvent(new IKUPMessageEvent(evnt.SenderID,transferee.ReciverID,$"{evnt.AmountOfMoney}"));
        
        WithdrawMoney(transferer,evnt.AmountOfMoney);
        DespoitMoney(transferee, evnt.AmountOfMoney);
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

    #endregion
    public KUPFaction? GetFaction(string name)
    {
        return Factions.First(x => x.Name == name);
    }
    
    public KUPFaction? GetFaction(int ID)
    {
        return Factions.First(x => x.FactionID == ID);
    }


    public bool CouldCaptureSystem(KUPFilledSystem system)
    {
        if (system.SystemsPrimaryStation.PrimaryStationAsset.Controller != null &&
            system.SystemsPrimaryStation.PrimaryStationAsset.Controller.FactionType != FactionType.Unclaimed)
        {
        
        var locX = system.DisplayX;
        var locY = system.DisplayY;
        var ships = AssetsInPlay.Where(x => x.Location.SystemX == locX && x.Location.SystemY == locY);
        return ships.Any(x => x.Controller == system.SystemsPrimaryStation.PrimaryStationAsset.Controller);
    }
        return false;
    }
}