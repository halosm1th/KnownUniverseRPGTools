using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPTakeSystemEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int AssetWhichTookID { get; }
    public int TargetID => 1919991701;
    public int SystemStationID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        
        var ship = (game.GetAssetFromID(AssetWhichTookID) as KUPCombatAsset);
        if (!ship.AssetHasActed())
        {
            var taker = EventService.GetActorBySenderID(SenderID);
            var targetStation = game.AssetsInPlay.First(x => x.assetID == SystemStationID);
            var takeFact = game.Factions.First(x => x == taker);
            takeFact.AddAsset(targetStation);
            
            
            EventService.AddEvent(new KUPMoneyDepositEvent(
                game.SenderID, SenderID,targetStation.MoneyIncome));
        }
        
        
    }

    public KUPTakeSystemEvent(int senderId, int systemStationID, int takingAssetID)
    {
        SenderID = senderId;
        SystemStationID = systemStationID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        AssetWhichTookID = takingAssetID;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [System Take Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Has taken over system:  {SystemStationID}";
    }
}