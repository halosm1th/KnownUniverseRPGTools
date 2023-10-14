    using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

    namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPMoneyWithdrawEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }
    public bool HasBeenChecked { get; set; } = false;
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var sender = EventService.GetActorBySenderID(SenderID);
        var bank = game.Factions.First(x => x.Name == "Bank");
        EventService.AddEvent(new KUPWithdrawMessageEvent(SenderID, 
            bank.ReceiverID, $"{AmountOfMoney}"));

        game.WithdrawMoney(sender, AmountOfMoney);
        game.DepositMoney(bank, AmountOfMoney);
    }

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