using KnownUniversePoliticsGameWebApp.Pages;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPPlayer : IKUPEventActor
{
    public string Name { get; }
    public string Password { get; }
    public KUPFaction Faction { get; set; }
    public int PersonalFunds { get; set; }
    public int SenderID { get; }
    public int ReciverID { get; }
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public KUPPlayer(string name, string password, int personalFunds, int senderId, int reciverId)
    {
        Name = name;
        Password = password;
        PersonalFunds = personalFunds;
        SenderID = senderId;
        ReciverID = reciverId;
        AddToEventService();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the faction this player used to be</returns>
    public KUPFaction ChangeFaction(KUPFaction newFaction)
    {
        var oldFaction = Faction;

        Faction = newFaction;
        Faction.NewPlayer(this);
        
        return oldFaction;

    }
}