namespace KnownUniverseCombatTester;

public class KUWeapon
{
    public string Name { get; }
    public int CostInCredits { get; }
    public KUWeaponQuality Quality { get; }
    public int TechLevel { get; }

    public KUWeaponType Type{ get; }
    public KUWeaponRange Range { get; }
    public KUAmmuntion Ammo { get; }
    public int Capacity { get; }
    public int RateOfFire { get; }
    public KUWeaponTag Tags { get; }

    public KUWeapon(string name, int costInCredits, KUWeaponQuality quality, int techLevel, KUWeaponType type, KUWeaponRange range, KUAmmuntion ammo, int capacity, int rateOfFire, KUWeaponTag tags)
    {
        Name = name;
        CostInCredits = costInCredits;
        Quality = quality;
        TechLevel = techLevel;
        Type = type;
        Range = range;
        Ammo = ammo;
        Capacity = capacity;
        RateOfFire = rateOfFire;
        Tags = tags;
    }

    public int RollDamage(int modifier = 0)
    {
        return Ammo.CalculateDamage()+modifier;
    }
}