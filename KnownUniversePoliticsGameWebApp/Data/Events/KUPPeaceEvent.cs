using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPPeaceEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var enemy = game.GetFaction(ShipToDelete);
        var me = game.GetFaction(SenderID);

        me.Peace(enemy);
    }

    public KUPPeaceEvent(int senderId, int targetId)
    {
        SenderID = senderId;
        ShipToDelete = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Peace Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} ";
    }
}