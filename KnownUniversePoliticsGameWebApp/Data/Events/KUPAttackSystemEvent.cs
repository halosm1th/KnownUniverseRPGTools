namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAttackSystemEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int ShipOrdered { get; }
    private KUPLocation _location;
    public DateTime CreationTime { get; }

    public KUPAttackSystemEvent(int senderId, int shipOrdered, KUPLocation location)
    {
        SenderID = senderId;
        ShipOrdered = shipOrdered;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        _location = location;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [System Attack Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} "+
               $"@ {_location}";
    }
}