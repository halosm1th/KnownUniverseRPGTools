namespace KnownUniverseCombatTester;

public class KUCharacter
{
    
    public string Name { get; }
    public int Brawn { get; }
    public int Reflex { get; }
    public int Endurnace { get; }
    public int GunCombat { get; }
    public int MeleeCombat { get; }
    
    public KUArmour Armour { get; }
    public KUWeapon Weapon { get; }

    public KUCharacter(string name, int brawn, int reflex, int endurnace, int gunCombat, int meleeCombat, KUArmour armour, KUWeapon weapon)
    {
        Name = name;
        Brawn = brawn;
        Reflex = reflex;
        Endurnace = endurnace;
        GunCombat = gunCombat;
        MeleeCombat = meleeCombat;
        Armour = armour;
        Weapon = weapon;
    }

    public int RollDamage()
    {
        var damage = Weapon.RollDamage();
        return damage;
    }
}