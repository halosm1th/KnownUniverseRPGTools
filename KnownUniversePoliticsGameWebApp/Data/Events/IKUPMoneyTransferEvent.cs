using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class IKUPMoneyTransferEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }


    public int TargetAccountID { get; }
    public int AmountOfMoney { get; }

    public IKUPMoneyTransferEvent(int senderId, int targetAccountId, int amountOfMoney)
    {
        SenderID = senderId;
        TargetAccountID = targetAccountId;
        AmountOfMoney = amountOfMoney;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }
    
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var transferer = EventService.GetActorBySenderID(SenderID);
        var transferee = EventService.GetActorByReciverID(TargetAccountID);

        game.WithdrawMoney(transferer, AmountOfMoney);
        game.DepositMoney(transferee, AmountOfMoney);
    }
    

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Transfer Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"${AmountOfMoney}";
    }
}