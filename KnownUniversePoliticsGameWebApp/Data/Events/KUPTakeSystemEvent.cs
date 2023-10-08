namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPTakeSystemEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int AssetWhichTookID { get; }
    public int TargetID => 1919991701;
    public int SystemStationID { get; }
    public DateTime CreationTime { get; }

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