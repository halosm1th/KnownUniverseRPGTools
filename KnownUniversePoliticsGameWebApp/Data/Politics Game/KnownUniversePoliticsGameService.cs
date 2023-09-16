using Simple_Subsector_Generator;
using SixLabors.ImageSharp;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KnownUniversePoliticsGameService
{

    public KnownUniversePoliticsGame PoliticsGame { get; protected set; }

    public bool HasBeenInited = false;
    
    
    public static string Base64Image = "";
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
        //Referesh the map every 2 minutes, saves on processing power.
        if (DateTime.Now >= ImageCreated.AddMinutes(5) || RefereshMap)
        {
            Base64Image = String.Empty;
            RefereshMap = false;
        }

        if (Base64Image == String.Empty)
        {
            using (Image img = GetFulLMap())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await img.SaveAsPngAsync(ms);
                    byte[] imgBytes = ms.ToArray();
                    Base64Image =  Convert.ToBase64String(imgBytes);
                    ImageCreated = DateTime.Now;
                }
            }
        }
        
        return Base64Image;
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

    public void EndOfTurn()
    {
        PoliticsGame.EndOfTurn();
    }

    public KUPFaction? GetFaction(string name)
    {
        return PoliticsGame.GetFaction(name);
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

    public bool CouldCaptureSystem(KUPFilledSystem system)
    {
        return PoliticsGame.CouldCaptureSystem(system);
    }

    public KUPCombatAsset GetShip(int shipAssetId)
    {
        var ships = PoliticsGame.AssetsInPlay.First(x => x.assetID == shipAssetId);
        return ships as KUPCombatAsset;
    }
}