using System.Collections;
using System.Diagnostics;

namespace KnownUniversePoliticsGameWebApp.Data;

public class KUPCombatAsset : IKUPAsset, IKUPEventActor
{
    public int HP { get; set; }
    public bool HasMoved { get; set; } = false;
    public bool HasActed { get; set; } = false;
    public int assetID { get; }
    public string Name => "Spaceship [" + Size + "] (" + Controller.Name + ") @ " + Location;
    public int SenderID { get; }
    public int ReceiverID { get; }
    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public KUPCombatAsset(KUPLocation location, KUPFaction controller, CombatAssetSize size, int id, int evntHandlingID)
    {
        Location = location;
        Controller = controller;
        Size = size;
        assetID = id;
        SenderID = evntHandlingID;
        ReceiverID = evntHandlingID;
        HP = Size switch
        {
            CombatAssetSize.Small => 1,
            CombatAssetSize.Medium => 2,
            CombatAssetSize.Large => 3,
            CombatAssetSize.Station => 4,
        };
        
        AddToEventService();
    }

    public KUPLocation Location { get; private set; }
    public KUPFaction Controller { get; set; }
    CombatAssetSize Size { get; }

    int IKUPAsset.MoralIncome => 0;

    private int _MoneyTotal {
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                return 10;
            }
            else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    return 20;
                }

                if (HP == 1)
                {
                    return 10;
                }
            }
            else if (Size == CombatAssetSize.Large || Size == CombatAssetSize.Station)
            {
                if (HP == 5)
                {
                    return 80;
                }
                
                if (HP == 4)
                {
                    return 50;
                }

                if (HP == 3)
                {
                    return 35;
                }

                if (HP == 2)
                {
                    return 20;
                }

                if (HP == 1)
                {
                    return 10;
                }
            }

            return 5;
        }
    }
        
    int _influecnceTotal{
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                return 5;
            }else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    return 15;   
                }

                if (HP == 2)
                {
                    return 10;
                }
            }else if (Size == CombatAssetSize.Large || Size == CombatAssetSize.Station)
            {
                if (HP == 5)
                {
                    return 45;
                }
                if (HP == 4)
                {
                    return 30;
                }
                
                if (HP == 3)
                {
                    return 25;   
                }
                if (HP == 2)
                {
                    return 20;   
                }

                if (HP == 1)
                {
                    return 15;
                }
            }

            return 5;
        }
    }

    public int MoneyTotal => _MoneyTotal;

    public int InfluenceTotal => _influecnceTotal;
    int IKUPAsset.MoneyIncome => 0;

    int IKUPAsset.UpKeepCost => MoneyTotal;

    int IKUPAsset.MoralCost => InfluenceTotal;

    public int AttackPower => Size switch
    {
        CombatAssetSize.Small => 1,
        CombatAssetSize.Medium => 2,
        CombatAssetSize.Large => 3,
        CombatAssetSize.Station => 5,
        _ => 1,
    };

    public void TakeDamage(int evntAmountOfDamage)
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
            if (x0 >1 && y0 > 1 && !movable.Any(x => x.SystemX == x0 && x.SystemY == y0))
            {
                var newLocs = new KUPLocation(x0, y0);
                var newDepth = depth - 1;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x1 >= xMIN && y0 >= yMIN && !movable.Any(x => x.SystemX == x1 && x.SystemY == y0))
            {
                
                var newLocs = new KUPLocation(x1, y0);
                var newDepth = depth - 1;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }
            if (x2 < XMAX && y0 > yMIN && !movable.Any(x => x.SystemX == x2 && x.SystemY == y0))
            {
                
                var newLocs = new KUPLocation(x2, y0);
                var newDepth = depth - 1;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x0 >= xMIN && y1 < yMAX && !movable.Any(x => x.SystemX == x0 && x.SystemY == y1))
            {
                
                var newLocs = new KUPLocation(x0, y1);
                var newDepth = depth - 1;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }
            if (x1 > xMIN && y2 < yMAX && !movable.Any(x => x.SystemX == x1 && x.SystemY == y2))
            {
                
                var newLocs = new KUPLocation(x1, y2);
                var newDepth = depth - 1;
                movable.Add(newLocs);
                GetMoveLocations(newLocs,movable,newDepth);
            }

            if (x2 < XMAX && y1 < yMAX && !movable.Any(x => x.SystemX == x2 && x.SystemY == y1))
            {
                
                var newLocs = new KUPLocation(x2, y1);
                var newDepth = depth - 1;
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

    
    public bool AssetHasActed()
    {
        if (!HasActed)
        {
            HasActed = true;
            return false;
        }

        return true;
    }
    
    public bool ChangeLocationTo(KUPLocation location)
    {
        if (!HasMoved)
        {
            var moveLocs = new List<KUPLocation>();
            GetMoveLocations(Location,
                moveLocs, MoveSpeed());
            if (moveLocs.Any(x => x == location))
            {
                Location = location;
                HasMoved = true;
                return true;
            }
        }

        return false;
    }
    
    public List<KUPLocation> MoveLocations()
    {
        int maxX = 8 * 4;
        int maxY = 10 * 4;
        var locs = new List<KUPLocation>();

        GetMoveLocations(this.Location,locs,MoveSpeed());
        
        //for()

        locs = locs.OrderBy(x => x.SystemX).ToList();
        
        return locs;
    }

    public static int GetCosts(CombatAssetSize size)
    {        return size switch
        {
            CombatAssetSize.Large => 250,
            CombatAssetSize.Medium => 125,
            CombatAssetSize.Small => 75,
            CombatAssetSize.Station => 175
        };
    }

    public override string ToString()
    {
        //Show them as a cost, by making it a negative thing.
        return $"{Name} HP: {HP} ${0-MoneyTotal} 😊{0-InfluenceTotal}";
    }

    public bool AtWar(KUPCombatAsset asset)
    {
        if (Controller.FactionRelationships[asset.Controller] == FactionRelationshipOptions.War) return true;
        if (asset.Controller == Controller) return false;

        return false;
    }
}