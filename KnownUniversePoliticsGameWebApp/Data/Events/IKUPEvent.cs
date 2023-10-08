namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPEvent
{
    int eventID { get; }
    int SenderID { get; }
    int TargetID { get; }
    DateTime CreationTime { get; }
}

public enum KUPOPerationSize
{
    Small, Medium, Large
}

public enum KUPOperationType
{
    InfluenceAttack, MoneyAttack, MilitaryAttack
}