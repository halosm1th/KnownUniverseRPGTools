namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPEvent
{
    int eventID { get; }
    int SenderID { get; }
    int TargetID { get; }
    DateTime CreationTime { get; }
}

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

public enum KUPOPerationSize
{
    Small, Medium, Large
}

public enum KUPOperationType
{
    InfluenceAttack, MoneyAttack, MilitaryAttack
}