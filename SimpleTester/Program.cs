// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata.Ecma335;
using KnownUniverseCombatTester;

namespace SimpleTester;

class CombatTest
{

    static List<SimpleKURPGWeapon> weapons = new ()
    { 
        new SimpleKURPGWeapon("Laser Sword", KURangeOrMelee.Melee, KUWeaponType.DirectEnergy,
            "2d10",0,2,1,KUWeaponRange.Melee,KUWeaponQuality.Good),
        
        new SimpleKURPGWeapon("Chainsword", KURangeOrMelee.Melee, KUWeaponType.Slug,
            "2d12",0,1,1,KUWeaponRange.Melee,KUWeaponQuality.Good),

        new ("Naval Pistol", KURangeOrMelee.Ranged, KUWeaponType.Slug,"2d6",4,
            2, 21, KUWeaponRange.Short, KUWeaponQuality.Military),
        
        new SimpleKURPGWeapon("Crossmus AR-51 Civilian Rifle",
            KURangeOrMelee.Ranged, KUWeaponType.Slug,"5d8",0,2,30,
    KUWeaponRange.Long,KUWeaponQuality.Good),
        
        new SimpleKURPGWeapon("NearTek AR-52 Assualt Rifle",
            KURangeOrMelee.Ranged, KUWeaponType.Slug,"4d8",0,3,60,
            KUWeaponRange.Average,KUWeaponQuality.Military),
        
        new SimpleKURPGWeapon("Redarra R-30 Rifle",
            KURangeOrMelee.Ranged,KUWeaponType.LaserEnergy,"4d6",0,3,240,
            KUWeaponRange.Average,KUWeaponQuality.Normal),
        
        new SimpleKURPGWeapon("Redarra R-31 Assualt Rifle",
            KURangeOrMelee.Ranged,KUWeaponType.LaserEnergy,"4d6",0,4,240,
            KUWeaponRange.Short,KUWeaponQuality.Good),
        
        new ("NearTek Heavy SMG",KURangeOrMelee.Ranged,KUWeaponType.Slug,"3d8",
            0,2,45,KUWeaponRange.Average,KUWeaponQuality.Military),
        new ("GenEng-Hasbrom Directed Rifle", KURangeOrMelee.Ranged, KUWeaponType.DirectEnergy, "4D10",
            0,2,150,KUWeaponRange.Extreme,KUWeaponQuality.Good),
        
        new ("Redarra-P21 Light Laser Pistol", KURangeOrMelee.Ranged,KUWeaponType.LaserEnergy,
            "3D4", 0, 3, 20, KUWeaponRange.Point_Blank,KUWeaponQuality.Poor),
        new ("Colat Light Pistol", KURangeOrMelee.Ranged, KUWeaponType.Slug, "2D6", 0,3,
            7,KUWeaponRange.Point_Blank, KUWeaponQuality.Poor),
        
        new ("Colat Medium Pistol",KURangeOrMelee.Ranged,KUWeaponType.Slug,"3d6",0,
            2,8,KUWeaponRange.Close,KUWeaponQuality.Normal),
        new ("Redarra-P22 Medium Laser Pistol", KURangeOrMelee.Ranged, KUWeaponType.LaserEnergy,"3d4",
            0,4,30,KUWeaponRange.Point_Blank,KUWeaponQuality.Normal),
    };

    private static List<SimpleKURPGArmour> armours = new List<SimpleKURPGArmour>()
    {
        new SimpleKURPGArmour(12,"Colonial",KUWeaponType.Slug),
        new SimpleKURPGArmour(14, "Flack Armour", KUWeaponType.Slug),
        new SimpleKURPGArmour(18, "Combat Armour", KUWeaponType.Slug),
        new SimpleKURPGArmour(24, "Heavy Armour", KUWeaponType.Slug),
        new SimpleKURPGArmour(30, "Power Armour", KUWeaponType.Slug),
        
        new (6,"Cheap Shield Laser",KUWeaponType.LaserEnergy),
        new (10,"Common Shield Laser",KUWeaponType.LaserEnergy),
        new (12,"Corporate Shield Laser",KUWeaponType.LaserEnergy),
        new (18,"Army Shield Laser",KUWeaponType.LaserEnergy),
        new (20,"Navy Shield Laser",KUWeaponType.LaserEnergy),
        new (22,"Fine Shield Laser",KUWeaponType.LaserEnergy),
        new (30,"Noble Energy Laser",KUWeaponType.DirectEnergy),
        new (40,"Witch Magic Laser",KUWeaponType.Void),
    };

    private static SimpleKURPGArmour FindArmour(string name) => new SimpleKURPGArmour(armours.First(x => x.Name == name).SP,armours.First(x => x.Name == name).Name,armours.First(x => x.Name == name).DamageType) ; 
    private static SimpleKURPGWeapon FindWeapon(string name) => weapons.First(x => x.Name == name); 


    public static void Main()
    {
        int c1Win = 0;
        int c2Win = 0;
        int numberOfTests = 50000;


        var range = KUWeaponRange.Short;

        var c1Name = "Jimmy";
        var c2Name = "Franky";
        var c1Weapon = "NearTek AR-52 Assualt Rifle";
        var c2Weapon = "NearTek AR-52 Assualt Rifle";
        var c1Armour = "Combat Armour";
        var c2Armour = "Combat Armour";
        
        
        for (int i = 0; i < numberOfTests; i++)
        {

            /*
        var C1 = new SimpleKURPGCharacter(9, 8, 7, 0, 2, 1,
            FindWeapon(c1Weapon), FindArmour(c1Armour), "Jimmy");
        var C2 = new SimpleKURPGCharacter(9, 8, 7, 0, 2, 1,
            FindWeapon(c2Weapon), FindArmour(c2Armour), "Franky");
*/
        var C1 = new SimpleKURPGCharacter(7, 7, 7, 0, 2, 1,
            FindWeapon(c1Weapon), FindArmour(c1Armour), c1Name);
        var C2 = new SimpleKURPGCharacter(7, 7, 7, 0, 2, 1,
            FindWeapon(c2Weapon), FindArmour(c2Armour), c2Name);
        
        var combat = new SimpleKURRPGCombat();

            combat.Init(C1, C2, range);
            var C1Won = combat.RunCombat();
            if (C1Won) c1Win++;
            else c2Win++;

            
            DisplayWinRate(c1Win, i, c2Win, c1Weapon, c1Armour, c2Weapon, c2Armour, c1Name, c2Name);
        }

        DisplayWinRateFinal(c1Win, numberOfTests, c2Win, c1Weapon, c1Armour, c2Weapon, c2Armour, c1Name, c2Name);
    }

    private static void DisplayWinRateFinal(int c1Win, int numberOfTests, int c2Win, string c1Weapon, string c1Armour,
        string c2Weapon, string c2Armour, string c1Name, string c2Name)
    {
        var c1DamagePercent = ((float) c1Win / (float) numberOfTests) * 100;
        var c2DamagePercent = ((float) c2Win / (float) numberOfTests) * 100;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(
            $"{c1Name} attacked with {c1Weapon} while wearing {c1Armour} and won {c1Win}/{numberOfTests} ({c1DamagePercent}%). " +
            $"\n{c2Name} attacked with {c2Weapon} while wearing {c2Armour} and won {c2Win}/{numberOfTests} ({c2DamagePercent}%).");
        if (c1Win > c2Win)
        {
            if ((c1Win / numberOfTests) > numberOfTests / 2)
            {
                Console.WriteLine($"{c1Name} won at least 50% if not more often.");
            }
            else
            {
                Console.WriteLine($"{c1Name} won more often.");
            }
        }
        else if (c2Win > c1Win)
        {
            if ((c2Win / numberOfTests) > numberOfTests / 2)
            {
                Console.WriteLine($"{c2Name} won at least 50% if not more often.");
            }
            else
            {
                Console.WriteLine($"{c2Name} won more often.");
            }
        }
        
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    
    private static void DisplayWinRate(int c1Win, int currentNumber, int c2Win, string c1Weapon, string c1Armour,
        string c2Weapon, string c2Armour, string c1Name,string c2Name)
    {
        var c1DamagePercent = ((float) c1Win / (float) currentNumber) * 100;
        var c2DamagePercent = ((float) c2Win / (float) currentNumber) * 100;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            $"{c1Name} has won {c1Win}/{currentNumber} ({c1DamagePercent}%). " +
            $"{c2Name} has won {c2Win}/{currentNumber} ({c2DamagePercent}%).");
       
        
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}