using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPOperationEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public int OperationTarget { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var amountOfDamage = game.GetAmountOfOperationDamage(OperationSize);
        var operationCost = game.GetOperationCost(OperationSize, OperationNumber);
        var target = game.Factions.First(x => x.FactionID == OperationTarget);

        var send = game.GetFaction(SenderID);
        ApplyCost(send, operationCost, OperationNumber);
        ApplyDamage(target, amountOfDamage, OperationNumber);

        EventService.AddEvent(
            new IKUPMessageEvent(SenderID,
                OperationTarget, OperationMessage));
    }
    
    private void ApplyCost(KUPFaction faction, int operationCost, KUPOperationType evntOperationNumber)
    {
        if (evntOperationNumber == KUPOperationType.InfluenceAttack)
        {
            faction.DamageInfluence(operationCost);
        }
        else if (evntOperationNumber == KUPOperationType.MoneyAttack)
        {
            faction.DamageMoney(operationCost);
        }
        else if (evntOperationNumber == KUPOperationType.MilitaryAttack)
        {
            faction.DamageInfluence(operationCost);
            faction.DamageMoney(operationCost);
        }
    }
    
    private void ApplyDamage(KUPFaction faction, int amountOfDamage, KUPOperationType type)
    {
        if (type == KUPOperationType.InfluenceAttack)
        {
            faction.DamageInfluence(amountOfDamage);
        }
        else if (type == KUPOperationType.MoneyAttack)
        {
            faction.DamageMoney(amountOfDamage);
        }
        else if (type == KUPOperationType.MilitaryAttack)
        {
            faction.DamageMilitary(amountOfDamage);
        }
    }

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