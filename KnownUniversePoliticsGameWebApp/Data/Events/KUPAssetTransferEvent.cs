using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPAssetTransferEvent : IKUPEvent
{
    public int eventID { get; }
    public int SenderID { get; }
    public int TargetID => 1919991701;
    public DateTime CreationTime { get; }
    public void RunEvent(KnownUniversePoliticsGame game, KUPEventService EventService)
    {
        
        var sender = game.Factions.First(x => x.SenderID == SenderID);
        var reciver = game.Factions.First(x => TargetFactionReciverID == x.ReceiverID);
        var targetAssetIDs = AssetsToTransfer;
        var assetsToTransfer = new List<IKUPAsset>();

        foreach (var asset in sender.Assets)
        {
            if (targetAssetIDs.Contains(asset.assetID))
            {
                assetsToTransfer.Add(asset);
            }
        }

        foreach (var asset in assetsToTransfer)
        {
            sender.DestroyAsset(asset);
            reciver.AddAsset(asset);
        }

        EventService.AddEvent(new IKUPMessageEvent(SenderID, TargetFactionReciverID, $"{sender.Name} " +
            $"has given you the following assets w/ IDs: {targetAssetIDs.Aggregate("", (h, t) => h + ", " + t)}"));

        EventService.AddEvent(new IKUPMessageEvent(TargetFactionReciverID, SenderID, $"{reciver.Name} " +
            $"has received the assets you gave them  w/ the following IDs: {targetAssetIDs.Aggregate("", (h, t) => h + ", " + t)}"));

    }

    public int TargetFactionReciverID { get; }
    public List<int> AssetsToTransfer { get; }

    public KUPAssetTransferEvent(int senderId, int targetFactionReciverID, List<int> assetsToTransfer)
    {
        SenderID = senderId;
        TargetFactionReciverID = targetFactionReciverID;
        AssetsToTransfer = assetsToTransfer;
        eventID = KUPEventService.GetEventID();
        CreationTime = DateTime.Now;
        ;
    }

    public override string ToString()
    {
        return $"#{eventID} ({CreationTime.ToLocalTime()}) - [Transfer Event] " +
               $"F:{KUPEventService.GetActorBySenderIDStatic(SenderID).Name} " +
               $"T:{KUPEventService.GetActorByReciverIDStatic(TargetID).Name} " +
               $"Assets ID #s: {AssetsToTransfer.Aggregate("", (h,t) => h + ", " + t)}";
    }
}