using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPWarDeclareEvent : IKUPEvent
{
    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var sender = game.GetFaction(SenderID);
        var reciever = game.GetFaction(TargetID);
        
        sender.AtWar(reciever);
        //Send the event that the war started to the person its actually declared against.
        EventService.AddEvent(
            new KUPAtWarEvent(reciever.SenderID,reciever.ReceiverID,
                sender.ReceiverID));
        
        var allies = sender.FactionRelationships
            .Where(x => x.Value == FactionRelationshipOptions.TotalAlliance);
        
        var enemies = reciever.FactionRelationships
            .Where(x => x.Value == FactionRelationshipOptions.DefenceAlliance
                        || x.Value == FactionRelationshipOptions.TotalAlliance);

        foreach (var ally in allies)
        {
            EventService.AddEvent(
                new KUPAtWarEvent(sender.SenderID,ally.Key.ReceiverID,
                    reciever.ReceiverID));
        }

        foreach (var enemy in enemies)
        {
            //The reciver (whom the enemies are the allies) sends a mesasge to their ally, infomring them they are at 
            //war with the enemy, whom is recorded as sender.
            EventService.AddEvent(
                new KUPAtWarEvent(reciever.SenderID,enemy.Key.ReceiverID,
                    sender.ReceiverID));
        }
    }

    public KUPWarDeclareEvent(int senderId, int targetId)
    {
        SenderID = senderId;
        TargetID = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [War declared Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} ";
    }
}