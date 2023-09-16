namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPStoreSomeoneBought : IKUPEvent
{

    
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID { get; }
    public DateTime CreationTime { get; }

    public string BuyerName { get; }
    public StoreItems ItemToBuy { get; }
    public int Amount { get; }

    public bool HasBeenDone { get; set; } = false;
    
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
    
    public KUPStoreSomeoneBought(int senderId, int targetId, string buyerName, StoreItems itemToBuy, int amount)
    {
        SenderID = senderId;
        BuyerName = buyerName;
        ItemToBuy = itemToBuy;
        TargetID = targetId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Money Event] F:{SenderID} T:{BuyerName} {Amount} * {ItemToBuy}";
    }
}