    namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPMoneyWithdrawEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }

    public int TargetAccountID { get; }
    public int AmountOfMoney { get; }

    public KUPMoneyWithdrawEvent(int senderId, int targetAccountId, int amountOfMoney)
    {
        SenderID = senderId;
        TargetAccountID = targetAccountId;
        AmountOfMoney = amountOfMoney;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Withdraw Event]" +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"${AmountOfMoney}";
    }
}