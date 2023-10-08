namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAllianceEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }

    public KUPAllianceEvent(int senderId, int targetId)
    {
        SenderID = senderId;
        TargetID = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Alliance Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} ";
    }
}