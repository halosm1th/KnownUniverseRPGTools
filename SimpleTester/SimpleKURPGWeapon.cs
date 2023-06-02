using KnownUniverseCombatTester;

namespace SimpleTester;

class SimpleKURPGWeapon
{


    public KURangeOrMelee RangeOrMelee { get; }
    public string Name { get; }
    public string DamageDice { get; }
    public int RateOfFire { get; }
    public int Capacity { get; }
    public KUWeaponRange Range { get; }
    public KUWeaponQuality Quality { get; }
    public KUWeaponType DamageType { get; }
    public int BonusDamage { get; }

    public SimpleKURPGWeapon(string name, KURangeOrMelee rangeOrMelee,KUWeaponType damageType , string damage, int bonousDmg, int rateOfFire, int capacity, KUWeaponRange range, KUWeaponQuality quality)
    {
        Name = name;
        RangeOrMelee = rangeOrMelee;
        DamageDice = damage;
        BonusDamage = bonousDmg;
        DamageType = damageType;
        RateOfFire = rateOfFire;
        Capacity = capacity;
        Range = range;
        Quality = quality;
    }
}