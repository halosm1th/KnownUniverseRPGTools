namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPMoveAssetEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public KUPLocation Destination { get; }

    public KUPMoveAssetEvent(int senderId, int targetID, KUPLocation destination)
    {
        SenderID = senderId;
        TargetID = targetID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        Destination = destination;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Move Asset Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Moved asset to:  {Destination}";
    }
}