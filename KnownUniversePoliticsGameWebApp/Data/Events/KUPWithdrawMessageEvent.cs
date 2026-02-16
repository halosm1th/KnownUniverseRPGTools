using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPWithdrawMessageEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {

        Console.WriteLine($"Message from: {SenderID} to {ShipToDelete} saying: {Message}");
    }

    public string Message { get; }
    public bool HasBeenChecked { get; set; } = false;

    public KUPWithdrawMessageEvent(int senderId, int bank, string message)
    {
        eventID = KUPEventService.GetEventID();
        SenderID = senderId;
        ShipToDelete = bank;
        Message = message;
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) [Message] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} " +
               $"M: {Message}";
    }
}

public class IKUPMessageEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {

        Console.WriteLine($"Message from: {SenderID} to {ShipToDelete} saying: {Message}");
    }

    public string Message { get; }

    public IKUPMessageEvent(int senderId, int targetId, string message)
    {
        eventID = KUPEventService.GetEventID();
        SenderID = senderId;
        ShipToDelete = targetId;
        Message = message;
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) [Message] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} " +
               $"M: {Message}";
    }
}