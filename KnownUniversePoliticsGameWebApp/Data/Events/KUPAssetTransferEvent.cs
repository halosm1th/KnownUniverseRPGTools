namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAssetTransferEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }

    public int TargetFactionReciverID { get; }
    public List<int> AssetsToTransfer { get; }

    public KUPAssetTransferEvent(int senderId, int targetFactionReciverID, List<int> assetsToTransfer)
    {
        SenderID = senderId;
        TargetFactionReciverID = targetFactionReciverID;
        AssetsToTransfer = assetsToTransfer;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Transfer Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Assets ID #s: {AssetsToTransfer.Aggregate("", (h,t) => h + ", " + t)}";
    }
}