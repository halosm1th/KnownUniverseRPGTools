using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPTakeAssetEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int AssetID { get; }
    public int AssetWhichTookID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var ship = (game.GetAssetFromID(AssetWhichTookID) as KUPCombatAsset);
        
            var taker = EventService.GetActorBySenderID(SenderID);
            var targetStation = game.AssetsInPlay.First(x => x.assetID == AssetID);
            var takeFact = game.Factions.First(x => x == taker);
            takeFact.AddAsset(targetStation);
        
    }

    public KUPTakeAssetEvent(int senderId, int assetID, int takingAssetID)
    {
        SenderID = senderId;
        AssetID = assetID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        AssetWhichTookID = takingAssetID;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Asset Taken Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Has taken asset:  {AssetID}";
    }
}