namespace KnownUniverseCombatTester;

public class KUArmour
{
    public enum KUArmourLocation
    {
        Head,
        Body
    }
    
    public string Name { get; }
    public int Cost { get; }
    public int TechLevel { get; }
    public int StoppingPower { get; }
    public KUWeaponType ArmourType { get; }
    public KUArmourLocation Location { get; }
}