using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPShipDeleteEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        var ship = game.AssetsInPlay.OfType<KUPCombatAsset>()
            .First(x => x.ReceiverID == ShipToDelete);
        
        ship.Controller.DestroyAsset(ship);
    }

    public int AmountOfDamage { get; }

    public KUPShipDeleteEvent(int senderId, int shipToDelete)
    {
        SenderID = senderId;
        ShipToDelete = shipToDelete;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Ship Destroyed Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} ";
    }
}

public class KUPShipDamagedEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int ShipToDelete { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var ship = game.AssetsInPlay.OfType<KUPCombatAsset>()
            .First(x => x.ReceiverID == ShipToDelete);
        if (ship.HP - AmountOfDamage <= 0)
        {
            EventService.AddEvent(
                new IKUPMessageEvent(SenderID,
                    ShipToDelete, $"Ship was Destroyed."));

            EventService.AddEvent(
                new IKUPMessageEvent(ShipToDelete,
                    SenderID, $"Ship was Destroyed."));
            game.DestroyAsset(ship, ship.Controller);
        }
        else
        {
            ship.TakeDamage(AmountOfDamage);
            EventService.AddEvent(
                new IKUPMessageEvent(SenderID,
                    ShipToDelete, $"Took 1 point of damage."));
            EventService.AddEvent(
                new IKUPMessageEvent(ShipToDelete,
                    SenderID, $"Took 1 point of damage."));
        }
    }

    public int AmountOfDamage { get; }

    public KUPShipDamagedEvent(int senderId, int targetId, int amountOfDamage)
    {
        SenderID = senderId;
        ShipToDelete = targetId;
        AmountOfDamage = amountOfDamage;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Ship Damaged event Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(ShipToDelete).Name} " +
               $"Amount: {AmountOfDamage}";
    }
}