using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using KnownUniversePoliticsGameWebApp.Data;
using Simple_Subsector_Generator;

public class KUPFilledSystem : KUPStarSystem
{
    public List<KUPPointsOfInterest> PointsOfInterest { get; }
    public List<KUPPointsOfInterest> GetComplexPOI()
    {
        var complexPOI = new List<KUPPointsOfInterest>();
        foreach (var poi in PointsOfInterest)
        {
            if (poi.GetType() == typeof(KupPointsOfInterestAsteroid)
                || poi.GetType() == typeof(KupPrimaryStation)
                || poi.GetType() == typeof(KupPointsOfInterestWorld)
                || poi.GetType() == typeof(KupPointsOfInterestOther)
                || poi.GetType() == typeof(KupPointsOfInterestStation))
            {
                complexPOI.Add(poi);
            }
        }

        return complexPOI;
    }
    
    
    public List<KUPPointsOfInterest> GetPOI()
    {
        var pois = new List<KUPPointsOfInterest>();
        foreach (var poi in PointsOfInterest)
        {
            if (poi.GetType() == typeof(KupPointsOfInterestAsteroid))
            {
                    pois.Add(poi);
            }else if (poi.GetType() == typeof(KupPrimaryStation))
            {
                
                    pois.Add(poi);
            }
            else if (poi.GetType() == typeof(KupPointsOfInterestWorld))
            {
                
                    pois.Add(poi);
            }else if (poi.GetType() == typeof(KupPointsOfInterestOther))
            {
                
                    pois.Add(poi);
            }else if (poi.GetType() == typeof(KupPointsOfInterestStation))
            {
                    pois.Add(poi);
            }
        }
        return pois;
    }
    
    public List<KUPPointsOfInterest> GetUnclaimedPOI()
    {
        var unclaimed = new List<KUPPointsOfInterest>();
        foreach (var poi in PointsOfInterest)
        {
            if (poi.GetType() == typeof(KupPointsOfInterestAsteroid))
            {
                if ((poi as KupPointsOfInterestAsteroid).AsteroidAsset.Controller == null
                    || (poi as KupPointsOfInterestAsteroid).AsteroidAsset.Controller?.FactionType == FactionType.Unclaimed)
                {
                    unclaimed.Add(poi);
                }
            }else if (poi.GetType() == typeof(KupPrimaryStation))
            {
                
                if ((poi as KupPrimaryStation).PrimaryStationAsset.Controller == null
                    || (poi as KupPrimaryStation).PrimaryStationAsset.Controller.FactionType == FactionType.Unclaimed)
                {
                    unclaimed.Add(poi);
                }
            }
            else if (poi.GetType() == typeof(KupPointsOfInterestWorld))
            {
                
                if ((poi as KupPointsOfInterestWorld).WorldAsset.Controller == null
                        ||(poi as KupPointsOfInterestWorld).WorldAsset.Controller.FactionType == FactionType.Unclaimed)
                {
                    unclaimed.Add(poi);
                }
            }else if (poi.GetType() == typeof(KupPointsOfInterestOther))
            {
                
                if ((poi as KupPointsOfInterestOther).OtherAsset.Controller == null
                    || (poi as KupPointsOfInterestOther).OtherAsset.Controller.FactionType == FactionType.Unclaimed)
                {
                    unclaimed.Add(poi);
                }
            }else if (poi.GetType() == typeof(KupPointsOfInterestStation))
            {
                if ((poi as KupPointsOfInterestStation).StationAsset.Controller == null
                     || (poi as KupPointsOfInterestStation).StationAsset.Controller.FactionType == FactionType.Unclaimed)
                {
                    unclaimed.Add(poi);
                }
            }
        }
        return unclaimed;
    }
    
    public string Name { get; }

    public string USPDisplay()
    {
        
        var sb = new StringBuilder();

        sb.Append($"{GetSystemTradeCodesText()}\n");
        return sb.ToString() ;
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"{Name}: {GetSystemTradeCodesText()}\n");
        sb.Append($"_{GetTradecodeMeaning().Replace("None, ", "")}_\n");
        foreach (var poi in PointsOfInterest)
        {
            sb.Append(poi.ToString());
        }
        
        return base.ToString() + sb.ToString()+ "\n---";
    }

    public string GetTradecodeMeaning()
    {
        var sb = new StringBuilder();
        foreach (var tc in PointsOfInterest)
        {
            sb.Append($"{tc.GetTradecodeMeaning().Replace("None, ", "")}, ");
        }

        
        return sb.ToString();
    }

    public List<string> GetTradeCodesDisplay()
    {
        var tc = new List<string>();
        foreach (var poi in PointsOfInterest.Where(x => x.HasComplexInfo 
                                                         && x.GetType() != typeof(KupPointsOfInterestWreck)))
        {
            foreach (var code in poi.GetTradeCodeDisplay().Replace(" ","").Split(","))
            {
                if(!tc.Contains(code)) tc.Add(code);
            }
        }

        return tc;
    }
    

    
    private string GetSystemTradeCodesText()
    {
        var sb = new StringBuilder();
        sb.Append($"{SystemsPrimaryStation?.SubtypeName}{PointsOfInterest.Count} -");

        var text = GetTradeCodesDisplay().Aggregate("", (h, t) =>
        {
            var retText = "";
            foreach (var l in t.Split(','))
            {
                if (!h.Contains(l) && !retText.Contains(l)) retText += l;
            }
            
            return h += retText.Replace(" ","");
        });
            sb.Append($" {text}");
            return sb.ToString();
    }

    public KupPrimaryStation? SystemsPrimaryStation =>
        PointsOfInterest.First(x => x.GetType() == typeof(KupPrimaryStation)) as KupPrimaryStation;

    
    public KUPFilledSystem(string name, int x, int y, KURPGSubsector subsector, ref int dispX, ref int dispY) : base(x, y, subsector, ref dispX, ref dispY)
    {
        Name = name;
        PointsOfInterest = new List<KUPPointsOfInterest>();
    }

    public void AddPoint(KUPPointsOfInterest poi)
    {
        PointsOfInterest.Add(poi);
    }

    public List<IKUPAsset> POIAssets()
    {
        var assets = new List<IKUPAsset>();

        foreach (var poi in GetComplexPOI())
        {
            if (poi.GetType() == typeof(KupPointsOfInterestAsteroid))
                {
                    assets.Add((poi as KupPointsOfInterestAsteroid).AsteroidAsset);
                }else if (poi.GetType() == typeof(KupPrimaryStation))
                {
                    assets.Add((poi as KupPrimaryStation).PrimaryStationAsset);

                }
                else if (poi.GetType() == typeof(KupPointsOfInterestWorld))
                {
                    assets.Add((poi as KupPointsOfInterestWorld).WorldAsset);

                }else if (poi.GetType() == typeof(KupPointsOfInterestOther))
                {
                    assets.Add((poi as KupPointsOfInterestOther).OtherAsset);

                }else if (poi.GetType() == typeof(KupPointsOfInterestStation))
                {                   
                    assets.Add((poi as KupPointsOfInterestStation).StationAsset);
                }
        }

        return assets;
    }
}