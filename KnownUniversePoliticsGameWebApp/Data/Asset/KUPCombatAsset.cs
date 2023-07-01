using System.Collections;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPCombatAsset : IKUPAsset, IKUPEventActor
{
    public int HP { get; set; }
    public int assetID { get; }
    public string Name { get; set; }
    public int SenderID { get; }
    public int ReciverID { get; }
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public KUPLocation Location { get; }
    public KUPFaction Controller { get; set; }
    CombatAssetSize Size { get; }

    int IKUPAsset.MoralIncome => 0;

    private int _MoneyTotal = 0;
    private int _influecnceTotal = 0;

    public int MoneyTotal => _MoneyTotal;

    public int InfluenceTotal => _influecnceTotal;
    int IKUPAsset.MoneyIncome => 0;

    int IKUPAsset.UpKeepCost
    {
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                _MoneyTotal = 10;
                return 10;
            }
            else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    _MoneyTotal = 20;
                    return 20;
                }

                if (HP == 1)
                {
                    _MoneyTotal = 10;
                    return 10;
                }
            }
            else if (Size == CombatAssetSize.Large)
            {
                if (HP == 4)
                {
                    _MoneyTotal = 50;
                    return 50;
                }

                if (HP == 3)
                {
                    _MoneyTotal = 35;
                    return 35;
                }

                if (HP == 2)
                {
                    _MoneyTotal = 20;
                    return 20;
                }

                if (HP == 1)
                {
                    _MoneyTotal = 10;
                    return 10;
                }
            }

            _MoneyTotal = 5;
            return 5;
        }
    }

    int IKUPAsset.MoralCost
    {
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                _influecnceTotal = 5;
                return 5;
            }else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    _influecnceTotal = 10;
                    return 10;   
                }

                if (HP == 1)
                {
                    _influecnceTotal = 15;
                    return 15;
                }
            }else if (Size == CombatAssetSize.Large)
            {
                if (HP == 4)
                {
                    _influecnceTotal = 15;
                    return 15;
                }
                
                if (HP == 3)
                {
                    _influecnceTotal = 20;
                    return 20;   
                }
                if (HP == 2)
                {
                    _influecnceTotal = 25;
                    return 25;   
                }

                if (HP == 1)
                {
                    _influecnceTotal = 30;
                    return 30;
                }
            }

            _influecnceTotal = 5;
            return 5;
        }
    }

    public void DoDamage(int evntAmountOfDamage)
    {
        HP =- evntAmountOfDamage;
    }

    private void GetMoveLocations(KUPLocation currentLocation, List<KUPLocation> movable, int depth = 0)
    {
        int XMAX = 41;
        int yMAX = 33;
        int yMIN = 1;
        int xMIN = 1;
        if (depth > 0)
        {
            var x0 = currentLocation.SystemX - 1;
            var x1 = currentLocation.SystemX;
            var x2 = currentLocation.SystemX + 1;
            var y0 = currentLocation.SystemY - 1;
            var y1 = currentLocation.SystemY;
            var y2 = currentLocation.SystemY + 1;
            if (x0 >1 && y0 > 1)
            {
                var newLocs = new KUPLocation(x0, y0);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x1 >= xMIN && y0 >= yMIN)
            {
                
                var newLocs = new KUPLocation(x1, y0);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }
            if (x2 < XMAX && y0 > yMIN)
            {
                
                var newLocs = new KUPLocation(x2, y0);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x0 >= xMIN && y1 < yMAX)
            {
                
                var newLocs = new KUPLocation(x0, y1);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }
            if (x1 > xMIN && y2 < yMAX)
            {
                
                var newLocs = new KUPLocation(x1, y2);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x2 < XMAX && y1 < yMAX)
            {
                
                var newLocs = new KUPLocation(x2, y1);
                var newDepth = depth--;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

        }
    }

    public int MoveSpeed() =>
        Size switch
        {
            CombatAssetSize.Small => 2,
            CombatAssetSize.Medium => 4,
            CombatAssetSize.Large => 6,
            CombatAssetSize.Station => 1
        };

    public List<KUPLocation> MoveLocations()
    {
        int maxX = 8 * 4;
        int maxY = 10 * 4;
        var locs = new List<KUPLocation>();

        GetMoveLocations(this.Location,locs,MoveSpeed());
        
        //for()
        
        return locs;
    }
}