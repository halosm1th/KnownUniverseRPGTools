using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPEvent
{
    int eventID { get; }
    int SenderID { get; }
    int TargetID { get; }
    DateTime CreationTime { get; }

    void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService);
}

public enum KUPOPerationSize
{
    Small, Medium, Large
}

public enum KUPOperationType
{
    InfluenceAttack, MoneyAttack, MilitaryAttack
}