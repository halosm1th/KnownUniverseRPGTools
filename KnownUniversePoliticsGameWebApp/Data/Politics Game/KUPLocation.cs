using KnownUniversePoliticsGameWebApp.Data.Politics_Game;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPLocation
{
    public static KnownUniversePoliticsGame politicsGame;
    
    public int SystemX { get; set; }
    public int SystemY { get; set; }

    public KUPLocation(int systemX, int systemY)
    {
        SystemX = systemX;
        SystemY = systemY;
    }
    public static bool operator ==(KUPLocation obj1, KUPLocation obj2)
    {
        return obj1.SystemX == obj2.SystemX
               && obj1.SystemY == obj2.SystemY;

    }
    public static bool operator !=(KUPLocation obj1, KUPLocation obj2)
    {
        return obj1.SystemX != obj2.SystemX
               || obj1.SystemY != obj2.SystemY;

    }
    
    public override string ToString()
    {
        if (politicsGame != null)
        {

            return $"{politicsGame.GetLocationName(SystemX, SystemY)}  ({SystemX} {SystemY})";
        }
        else
        {
            return $"{SystemX} {SystemY}";
        }
    }
}