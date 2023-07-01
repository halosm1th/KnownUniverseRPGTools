namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPTakeAssetEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int AssetID { get; }
    public DateTime CreationTime { get; }

    public KUPTakeAssetEvent(int senderId, int assetID)
    {
        SenderID = senderId;
        AssetID = assetID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
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