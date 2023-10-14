using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPShipDamagedEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var ship = game.AssetsInPlay.OfType<KUPCombatAsset>()
            .First(x => x.ReceiverID == TargetID);
        if (ship.HP - AmountOfDamage <= 0)
        {
            EventService.AddEvent(
                new IKUPMessageEvent(SenderID,
                    TargetID, $"Ship was Destroyed."));

            EventService.AddEvent(
                new IKUPMessageEvent(TargetID,
                    SenderID, $"Ship was Destroyed."));
            game.DestroyAsset(ship, ship.Controller);
        }
        else
        {
            ship.TakeDamage(AmountOfDamage);
            EventService.AddEvent(
                new IKUPMessageEvent(SenderID,
                    TargetID, $"Took 1 point of damage."));
            EventService.AddEvent(
                new IKUPMessageEvent(TargetID,
                    SenderID, $"Took 1 point of damage."));
        }
    }

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