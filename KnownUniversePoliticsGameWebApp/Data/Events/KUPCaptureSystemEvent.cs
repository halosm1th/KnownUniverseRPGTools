using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPCaptureSystemEvent : IKUPEvent
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
        
            var taker = EventService.GetActorBySenderID(SenderID);
            var targetStation = game.AssetsInPlay.First(x => x.assetID == SystemStationID);
            var previousHolder = targetStation.Controller;
            previousHolder.DestroyAsset(targetStation);

            var takeFact = game.Factions.First(x => x == taker);
            takeFact.AddAsset(targetStation);


            EventService.AddEvent(new IKUPMessageEvent(
                taker.SenderID, previousHolder.ReceiverID,
                $"You have lost control of {targetStation.Location} to {taker.Name}"));
            
        
    }

    public KUPCaptureSystemEvent(int senderId, int systemStationID, int takingAssetID)
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
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [System Capture Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Has Captured system:  {SystemStationID}";
    }
}