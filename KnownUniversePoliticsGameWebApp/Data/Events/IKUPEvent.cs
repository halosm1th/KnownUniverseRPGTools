namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPEvent
{
    int eventID { get; }
    int SenderID { get; }
    int TargetID { get; }
    DateTime CreationTime { get; }
}