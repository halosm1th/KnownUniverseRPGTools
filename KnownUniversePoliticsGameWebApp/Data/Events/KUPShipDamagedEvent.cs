namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPShipDamagedEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public int AmountOfDamage { get; }

    public KUPShipDamagedEvent(int senderId, int targetId, int amountOfDamage)
    {
        SenderID = senderId;
        TargetID = targetId;
        AmountOfDamage = amountOfDamage;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Ship Damaged event Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Amount: {AmountOfDamage}";
    }
}