using KnownUniversePoliticsGameWebApp.Data.Politics_Game;
using KUP_Simple_Sector_Generator;
using SixLabors.ImageSharp;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KnownUniversePoliticsGameService
{

    public KnownUniversePoliticsGame PoliticsGame { get; protected set; }

    public bool HasBeenInited = false;
    
    
    public static byte[] Base64Image;
    private DateTime ImageCreated = DateTime.UnixEpoch;
    private bool RefereshMap = false;
    public async Task  Init()
    {
        if (!HasBeenInited)
        {
            PoliticsGame = new KnownUniversePoliticsGame();
            KUPEventService.politicsGame = PoliticsGame;
            HasBeenInited = true;
            MapRefresh();
            
        }
    }
    
    public KnownUniversePoliticsGame GetPoliticsGame()
    {
        return PoliticsGame;
    }

    public async Task MapRefresh()
    {
        RefereshMap = true;
        await GetFullMapBase64();
    }
    
    public async Task<string> GetFullMapBase64()
    {
        var path = "";
        //Refresh the map every 2 minutes, saves on processing power.
        if (DateTime.Now >= ImageCreated.AddMinutes(5) || RefereshMap)
        {
            byte[] imgBytes = new byte[0];
            RefereshMap = false;
            
            using (Image img = GetFulLMap())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await img.SaveAsPngAsync(ms);
                    imgBytes = ms.ToArray();
                    
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    path = Path.Combine(path, "img.png");
                    
                    File.WriteAllBytes(path,imgBytes);
                    
                    Base64Image =  imgBytes;
                    ImageCreated = DateTime.Now;
                }
            }
        }

        return path;
    }
    
    public Image GetFulLMap()
    {
        return PoliticsGame.SectorImage;
    }

    public bool Login(string username, string password)
    {
        var hasUser = PoliticsGame.Players.First(x => x.Name == username);
        if (hasUser != null)
        {
            if (password == hasUser.Password)
            {
                return true;
            }
        }
        
        return false;
    }

    public KUPPlayer? GetPlayer(string username)
    {
        if (PoliticsGame != null)
        {
            return PoliticsGame.Players.Find(x => x.Name == username);
        }

        return null;
    }

    
    public KUPPlayer? GetPlayer(int id)
    {
        if (PoliticsGame != null)
        {
            return PoliticsGame.Players.Find(x => x.SenderID == id);
        }

        return null;
    }
    
    public void EndOfTurn()
    {
        PoliticsGame.EndOfTurn();
    }

    public KUPFaction? GetFaction(string name)
    {
        return PoliticsGame.GetFaction(name);
    }

    public bool CanTrasnferAsset(int factionID, List<int> assetsToTrade)
    {
        var fac = PoliticsGame.GetFaction(factionID);
        foreach (var asset in assetsToTrade)
        {
            if (!fac.Assets.Any(x => x.assetID == asset))
                return false;
        }
        
        return true;
    }

    
    public KUPFaction? GetFaction(int factionID)
    {
        return PoliticsGame.GetFaction(factionID);
    }
    
    public KUPStarSystem GetSystem(int xloc, int yloc)
    {
        
        return PoliticsGame.Sector.GetSystems().First(x => x.DisplayY == yloc && x.DisplayX == xloc);
    }
    
    public KUPFilledSystem? FindSystem(int xloc, int yloc)
    {
        try
        {
            return PoliticsGame.Sector.FilledSystems?.First(x => x.DisplayY == yloc && x.DisplayX == xloc);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public IKUPAsset GetAsset(int assetId)
    {
        return PoliticsGame.AssetsInPlay.First(x => x.assetID == assetId);
    }

    public bool CouldCaptureSystem(KUPFilledSystem system, KUPFaction who)
    {
        return PoliticsGame.CouldCaptureSystem(system, who);
    }

    public KUPCombatAsset GetShip(int shipAssetId)
    {
        var ships = PoliticsGame.AssetsInPlay.First(x => x.assetID == shipAssetId);
        return ships as KUPCombatAsset;
    }

    public void AdminTransferAssets(KUPFaction targetFaction, List<IKUPAsset> assetsToTransfer)
    {
        PoliticsGame.AdminTranferAssets(targetFaction, assetsToTransfer);
    }

    public void SetPlayerFaction(string playerName, int factionId)
    {
        PoliticsGame.SetPlayerFaction(playerName,factionId);
    }

    public List<IKUPLocationAsset> GetBuildLocations(KUPFaction? faction)
    {
        return PoliticsGame.GetBuildLocations(faction);
    }

    public void CreateNewFaction(string name, int facID, FactionType factionType,
        int money, int influence, List<IKUPAsset> assets, string chosenPlayer, List<string> summary,
        List<string> goals)
    {

        var player = GetPlayer(chosenPlayer);
        PoliticsGame.CreateNewFaction(name,facID,factionType,money,influence,assets,player,summary,goals);
    }
}