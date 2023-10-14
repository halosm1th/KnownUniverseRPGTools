using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPBuildShipEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }

    public CombatAssetSize Size { get; set; }
    public int BuildLocationID { get; set; }


    public KUPBuildShipEvent(int senderId, CombatAssetSize shipSize,
        int buildLoctionID)
    {
        SenderID = senderId;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        Size = shipSize;
        BuildLocationID = buildLoctionID;
        ;
    }
    
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var cost = KUPCombatAsset.GetCosts(Size);
        var buildLoc = game.AssetsInPlay.First(x => x.assetID == BuildLocationID).Location;
        var buildFaction = game.Factions.First(x => x.SenderID == SenderID);

        if (buildFaction.Money < cost) return;
        buildFaction.Money = buildFaction.Money - cost;
        game.BuildShip(buildLoc,buildFaction,Size);
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Ship Build Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"Building: {Size} at {BuildLocationID}.";
    }
}