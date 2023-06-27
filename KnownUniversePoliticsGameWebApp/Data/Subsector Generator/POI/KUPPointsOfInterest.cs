using System.Text;

public abstract class KUPPointsOfInterest
{
    public KUPPoiTypes POIType { get; }
    public abstract bool HasComplexInfo { get; }
    public int SubtypeRoll { get; }
    public KURPGFilledSystem InSystem { get; }
    
    public abstract string? SubtypeName  { get; }
    public abstract string SubTypeDescription { get; }

    protected KUPPointsOfInterest(KUPPoiTypes poiType, int subtypeRoll, KURPGFilledSystem inSystem)
    {
        POIType = poiType;
        SubtypeRoll = subtypeRoll;
        InSystem = inSystem;
    }

    public abstract List<KURPGTradeCodes> GetTradeCodes();

    public string GetTradeCodeDisplay()
    {

        var sb = new StringBuilder();
        foreach (var tradecode in GetTradeCodes())
        {
            switch (tradecode)
            {
                case KURPGTradeCodes.Hp:
                    sb.Append("👨‍👩‍👧‍👦, ");
                    break;
                case KURPGTradeCodes.Lp:
                    sb.Append("🙅‍, ");
                    break;
                case KURPGTradeCodes.Ht:
                    sb.Append("📀, ");
                    break;
                case KURPGTradeCodes.Lt:
                    sb.Append("💾, ");
                    break;
                case KURPGTradeCodes.Hl:
                    sb.Append("⚖, ");
                    break;
                case KURPGTradeCodes.Ll:
                    sb.Append("🔫, ");
                    break;
                case KURPGTradeCodes.Ha:
                    sb.Append("🌎, ");
                    break;
                case KURPGTradeCodes.Ag:
                    sb.Append("🚜, ");
                    break;
                case KURPGTradeCodes.Ma:
                    sb.Append("🏭, ");
                    break;
                case KURPGTradeCodes.Ec:
                    sb.Append("💳, ");
                    break;
                case KURPGTradeCodes.Re:
                    sb.Append("⚒, ");
                    break;
                case KURPGTradeCodes.Ga:
                    sb.Append("🪐, ");
                    break;
                case KURPGTradeCodes.Gw:
                    sb.Append("🗃, ");
                    break;
                case KURPGTradeCodes.Rl:
                    sb.Append("⛪, ");
                    break;
                case KURPGTradeCodes.Is:
                    sb.Append("🏴, ");
                    break;
                case KURPGTradeCodes.Rs:
                    sb.Append("🔬, ");
                    break;
                case KURPGTradeCodes.Th:
                    sb.Append("🎩, ");
                    break;
                case KURPGTradeCodes.Sp:
                    sb.Append("♨, ");
                    break;
                case KURPGTradeCodes.Rf:
                    sb.Append("⛽, ");
                    break;
                case KURPGTradeCodes.Ms:
                    sb.Append("⚔, ");
                    break;
                case KURPGTradeCodes.Mb:
                    sb.Append("💂‍♂️, ");
                    break;
                case KURPGTradeCodes.Vg:
                    sb.Append("⭕, ");
                    break;
                case KURPGTradeCodes.Bh:
                    sb.Append("🎩, ");
                    break;
                case KURPGTradeCodes.Wh:
                    sb.Append("🕳, ");
                    break;
                case KURPGTradeCodes.Io:
                    sb.Append("🏴‍☠️, ");
                    break;
                case KURPGTradeCodes.Sc:
                    sb.Append("🎰, ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }        


        return sb.ToString();
    }

    public string GetTradecodeMeaning()
    {
        var sb = new StringBuilder();
        foreach(var tradecode in GetTradeCodes()){
            switch (tradecode)
            {
                case KURPGTradeCodes.Hp:
                    sb.Append("High Pop, ");
                    break;
                case KURPGTradeCodes.Lp:
                    sb.Append("Low Pop, ");
                    break;
                case KURPGTradeCodes.Ht:
                    sb.Append("High Tech, ");
                    break;
                case KURPGTradeCodes.Lt:
                    sb.Append("Low Tech, ");
                    break;
                case KURPGTradeCodes.Hl:
                    sb.Append("High Law Level, ");
                    break;
                case KURPGTradeCodes.Ll:
                    sb.Append("Low Law Level, ");
                    break;
                case KURPGTradeCodes.Ha:
                    sb.Append("Inhabited world, ");
                    break;
                case KURPGTradeCodes.Ag:
                    sb.Append("Agricultural World, ");
                    break;
                case KURPGTradeCodes.Ma:
                    sb.Append("Manufacturing World, ");
                    break;
                case KURPGTradeCodes.Ec:
                    sb.Append("Economic World, ");
                    break;
                case KURPGTradeCodes.Re:
                    sb.Append("Resource World, ");
                    break;
                case KURPGTradeCodes.Ga:
                    sb.Append("Gas Giant, ");
                    break;
                case KURPGTradeCodes.Gw:
                    sb.Append("Government World, ");
                    break;
                case KURPGTradeCodes.Rl:
                    sb.Append("Religious, ");
                    break;
                case KURPGTradeCodes.Is:
                    sb.Append("Illegal Station, ");
                    break;
                case KURPGTradeCodes.Rs:
                    sb.Append("Research Station, ");
                    break;
                case KURPGTradeCodes.Th:
                    sb.Append("Trade Hub, ");
                    break;
                case KURPGTradeCodes.Sp:
                    sb.Append("Pleasure Station, ");
                    break;
                case KURPGTradeCodes.Rf:
                    sb.Append("Refueling/Refining Station, ");
                    break;
                case KURPGTradeCodes.Ms:
                    sb.Append("Military Base, ");
                    break;
                case KURPGTradeCodes.Mb:
                    sb.Append("System Defense Station, ");
                    break;
                case KURPGTradeCodes.Vg:
                    sb.Append("Void Gate, ");
                    break;
                case KURPGTradeCodes.Bh:
                    sb.Append("Black Market Hub, ");
                    break;
                case KURPGTradeCodes.Wh:
                    sb.Append("Wormhole, ");
                    break;
                case KURPGTradeCodes.Io:
                    sb.Append("Illegal Outpost, ");
                    break;
                case KURPGTradeCodes.Sc:
                    sb.Append("Space Casino, ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (sb.ToString() == "") sb.Append("None, ");

        sb.Remove(sb.Length-2, 2);
        
        return sb.ToString();
    }
    public override string ToString()
    {
        return $"\n### **{(POIType.ToString().Contains("_")? POIType.ToString().Replace("_"," ") : POIType.ToString()) }" +
               $" - {SubtypeName}**" +
               $" ({GetTradeCodeDisplay().Aggregate("", (h,t) => h+ t+ "")}" +
               $")\n- {SubTypeDescription}";
    }
}