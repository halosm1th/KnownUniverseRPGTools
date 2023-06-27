using SixLabors.ImageSharp;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KnownUniversePoliticsGameService
{

    public KnownUniversePoliticsGame PoliticsGame { get; protected set; }

    public bool HasBeenInited = false;
    
    
    public static string Base64Image = "";
    private DateTime ImageCreated = DateTime.UnixEpoch;
    private bool RefereshMap = false;
    public void Init()
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

    public void MapRefresh()
    {
        RefereshMap = true;
        GetFullMapBase64();
    }
    
    public string GetFullMapBase64()
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
                    img.SaveAsPng(ms);
                    byte[] imgBytes = ms.ToArray();
                    Base64Image = Convert.ToBase64String(imgBytes);
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
        return PoliticsGame.Players.Find(x => x.Name == username);
    }

    public void EndOfTurn()
    {
        PoliticsGame.EndOfTurn();
    }
}