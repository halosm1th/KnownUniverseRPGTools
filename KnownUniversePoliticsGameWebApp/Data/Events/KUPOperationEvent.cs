namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPOperationEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int OperationTarget { get; }
    public DateTime CreationTime { get; }
    
    public KUPOPerationSize OperationSize { get; }
    public string OperationMessage { get; }
    public KUPOperationType OperationNumber { get; }


    public KUPOperationEvent(int senderId, int targetID, KUPOPerationSize operationSize,
        KUPOperationType operationNumber, string operationText)
    {
        SenderID = senderId;
        OperationSize = operationSize;
        OperationNumber = operationNumber;
        OperationMessage = operationText;
        OperationTarget = targetID;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Operation Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Doing: {OperationNumber} of {OperationSize}. With message: {OperationMessage}";
    }
}