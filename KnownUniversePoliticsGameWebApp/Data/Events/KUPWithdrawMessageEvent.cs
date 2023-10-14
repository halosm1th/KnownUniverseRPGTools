using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPWithdrawMessageEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {

        Console.WriteLine($"Message from: {SenderID} to {TargetID} saying: {Message}");
    }

    public string Message { get; }
    public bool HasBeenChecked { get; set; } = false;

    public KUPWithdrawMessageEvent(int senderId, int bank, string message)
    {
        eventID = KUPEventService.GetEventID();
        SenderID = senderId;
        TargetID = bank;
        Message = message;
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) [Message] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"M: {Message}";
    }
}

public class IKUPMessageEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {

        Console.WriteLine($"Message from: {SenderID} to {TargetID} saying: {Message}");
    }

    public string Message { get; }

    public IKUPMessageEvent(int senderId, int targetId, string message)
    {
        eventID = KUPEventService.GetEventID();
        SenderID = senderId;
        TargetID = targetId;
        Message = message;
        CreationTime = DateTime.Now;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) [Message] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"M: {Message}";
    }
}