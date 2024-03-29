﻿using System.Text;
using KUP_Simple_Sector_Generator;

public class KURPGFilledSystem : KURPGStarSystem
{
    public List<KURPGPointsOfInterest> PointsOfInterest { get; }
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
                                                         && x.GetType() != typeof(KURPGPointsOfInterestWreck)))
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

    public KURPGPrimaryStation? SystemsPrimaryStation =>
        PointsOfInterest.First(x => x.GetType() == typeof(KURPGPrimaryStation)) as KURPGPrimaryStation;

    public KURPGFilledSystem(string name, int x, int y, KURPGSubsector subsector) : base(x, y, subsector)
    {
        Name = name;
        PointsOfInterest = new List<KURPGPointsOfInterest>();
    }

    public void AddPoint(KURPGPointsOfInterest poi)
    {
        PointsOfInterest.Add(poi);
    }
}