namespace KnownUniversePoliticsGameWebApp.Data;

public enum StoreItems
{
    Soda,
    Pizza,
    Chips,
    Beer,
    Hard_Liquer,
    Weed
}

public class KUPBuyStoreEvent : IKUPEvent
{

    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }
    public StoreItems ItemToBuy { get; }
    public int Amount { get; }
    
    public int Cost 
        => ItemToBuy switch
        {
            StoreItems.Soda => 1,
            StoreItems.Pizza => 5,
            StoreItems.Chips => 5,
            StoreItems.Beer => 10,
            StoreItems.Hard_Liquer => 50,
            StoreItems.Weed =>25, 
        };
    
    public KUPBuyStoreEvent(int senderId, StoreItems itemToBuy, int amount)
    {
        SenderID = senderId;
        ItemToBuy = itemToBuy;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        Amount = amount;
        ;
    }

    public static int GetCost(StoreItems buyItem) 
        => buyItem switch
        {
            StoreItems.Soda => 1,
            StoreItems.Pizza => 5,
            StoreItems.Chips => 5,
            StoreItems.Beer => 10,
            StoreItems.Hard_Liquer => 50,
            StoreItems.Weed =>25, 
        };
    
    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Money Event] F:{SenderID} ${ItemToBuy}";
    }
}