using KnownUniverseCombatTester;

namespace SimpleTester;

public class SimpleKURPGArmour
{
    public int SP { get; private set; }
    public string Name { get; }
    public KUWeaponType DamageType { get; }

    public void ReduceSP()
    {
        SP--;
    }

    public SimpleKURPGArmour(int sp, string name, KUWeaponType damageType)
    {
        SP = sp;
        Name = name;
        DamageType = damageType;
    }
}