namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPDefenseEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }

    public KUPDefenseEvent(int senderId, int targetId)
    {
        SenderID = senderId;
        TargetID = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Defense Pact Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} ";
    }
}