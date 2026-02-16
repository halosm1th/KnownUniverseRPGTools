using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPMoveAssetEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var target = (EventService.GetActorByReciverID(ShipToDelete) as KUPCombatAsset);
        if (!target?.ChangeLocationTo(Destination,game) ?? false)
        {
            EventService.AddEvent(new IKUPMessageEvent(SenderID, SenderID,
                "Error building your ship. The location was invalid."));
        }
    }

    public KUPLocation Destination { get; }

    public KUPMoveAssetEvent(int senderId, int targetID, KUPLocation destination)
    {
        SenderID = senderId;
        ShipToDelete = targetID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        Destination = destination;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Move Asset Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} " +
               $"Moved asset to:  {Destination}";
    }
}