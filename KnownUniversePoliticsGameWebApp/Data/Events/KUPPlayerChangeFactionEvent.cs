using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPPlayerChangeFactionEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        var player = game.Players.First(x => x.SenderID == SenderID);
        var fac = game.Factions.First(x => x.ReceiverID == ShipToDelete);

        var newFacPlayer = fac.Player;
        var oldFac = player.ChangeFaction(fac);
        fac.NewPlayer(player);
        oldFac.NewPlayer(null);
    }

    public KUPPlayerChangeFactionEvent(int playerID, int targetFactionId)
    {
        SenderID = playerID;
        ShipToDelete = targetFactionId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Change Faction Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} ";
    }
}