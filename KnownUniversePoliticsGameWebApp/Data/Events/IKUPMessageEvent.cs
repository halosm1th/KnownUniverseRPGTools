﻿namespace KnownUniversePoliticsGameWebApp.Data;

public class IKUPMessageEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
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