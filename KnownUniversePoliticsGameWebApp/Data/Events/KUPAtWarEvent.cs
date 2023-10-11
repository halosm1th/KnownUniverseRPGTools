using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAtWarEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public int WarAgainst { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var warAgainst = game.GetFaction(WarAgainst);
        var me = game.GetFaction(TargetID);

        warAgainst.AtWar(me);
        me.AtWar(warAgainst);
    }

    public KUPAtWarEvent(int senderId, int targetId, int against)
    {
        SenderID = senderId;
        TargetID = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        WarAgainst  = against;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [At war Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} "+
               $"A: {KUPEventService.GetActorByReciverIDStatic(WarAgainst).Name} ";
    }
}