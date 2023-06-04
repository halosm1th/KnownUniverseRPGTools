using System.Text;
using Simple_Subsector_Generator;

public class KURpgFilledSystem : KURpgStarSystem
{
    public List<KURpgPointsOfInterest> PointsOfInterest { get; }
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
                                                         && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck)))
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

    public KURpgPrimaryStation? SystemsPrimaryStation =>
        PointsOfInterest.First(x => x.GetType() == typeof(KURpgPrimaryStation)) as KURpgPrimaryStation;

    public KURpgFilledSystem(string name, int x, int y) : base(x, y)
    {
        Name = name;
        PointsOfInterest = new List<KURpgPointsOfInterest>();
    }

    public void AddPoint(KURpgPointsOfInterest poi)
    {
        PointsOfInterest.Add(poi);
    }
}