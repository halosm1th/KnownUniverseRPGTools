using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAllianceEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var enemy = game.GetFaction(ShipToDelete);
        var me = game.GetFaction(SenderID);

        me.Alliance(enemy);
    }

    public KUPAllianceEvent(int senderId, int targetId)
    {
        SenderID = senderId;
        ShipToDelete = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Alliance Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} ";
    }
}