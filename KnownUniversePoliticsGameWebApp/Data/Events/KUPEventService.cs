using System.ComponentModel;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPEventService
{
    public static KnownUniversePoliticsGame politicsGame { get; set; }
    
    public static bool hasBeenInit = false;
    public static List<IKUPEventActor> ActorList { get; set; } = new List<IKUPEventActor>()
    ;
    public static Queue<IKUPEvent> EventQueue { get; set; } = new Queue<IKUPEvent>();
    public static int currentEventID = 0;
    private OmniEvent Omni= new OmniEvent();

    public KUPEventService()
    {
    }

    public void Init()
    {
        KnownUniversePoliticsGame.EventService = this;
        hasBeenInit = true;
    }

    public static int GetEventID()
    {
        var id = currentEventID;
        currentEventID++;
        return id;
    }
    
    
    public IKUPEventActor GetActorByReciverID(int recieverID)
    {
        return ActorList.First(x => x.ReciverID == recieverID);
    }
    
    
    public IKUPEventActor GetActorBySenderID(int senderID)
    {
        return ActorList.First(x => x.SenderID == senderID);
    }

    public static IKUPEventActor GetActorByReciverIDStatic(int recieverID)
    {
        
        return ActorList.First(x => x.ReciverID == recieverID);
    }
    
    
    public static IKUPEventActor GetActorBySenderIDStatic(int senderID)
    {
        return ActorList.First(x => x.SenderID == senderID);
    }

    public static void AddActor(IKUPEventActor actor)
    {
        ActorList.Add(actor);
    }

    public void AddEvent(IKUPEvent evnt){
        EventQueue.Enqueue(evnt);
        politicsGame.ProcessEvent(evnt);
    }

    public List<IKUPEvent> GetEvents(int RecieverID)
    {
        return EventQueue.Where(x => x.TargetID == RecieverID || x.TargetID == -1).ToList();
    }
}

public interface IKUPEventActor
{
    string Name { get; }
    int SenderID { get; }
    int ReciverID { get; }

    void AddToEventService();
}

public class OmniEvent : IKUPEventActor
{
    public string Name => "Everyone";
    public int SenderID { get; }= -1;
    public int ReciverID { get; }= -1;

    public OmniEvent()
    {
        AddToEventService();
    }

    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }
}