namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPMoneyDepositEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }

    public int TargetAccountID { get; }
    public int AmountOfMoney { get; }

    public KUPMoneyDepositEvent(int senderId, int targetAccountId, int amountOfMoney)
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
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Money Event] F:{SenderID} T:{TargetAccountID} ${AmountOfMoney}";
    }
}