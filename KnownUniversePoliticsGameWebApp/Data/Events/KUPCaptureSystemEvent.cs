namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPCaptureSystemEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int SystemStationID { get; }
    public DateTime CreationTime { get; }

    public KUPCaptureSystemEvent(int senderId, int systemStationID)
    {
        SenderID = senderId;
        SystemStationID = systemStationID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
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