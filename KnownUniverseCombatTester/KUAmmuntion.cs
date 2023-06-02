namespace KnownUniverseCombatTester;

public class KUAmmuntion
{
    public string Name { get; }
    public int NumberOfDice { get; }
    public int SidesPerDie { get; }
    
    public int CostPerRound { get; }
    public KUWeaponType Type { get; }

    public KUAmmuntion(string name, int numberOfDice, int sidesPerDie, int costPerRound, KUWeaponType type)
    {
        Name = name;
        NumberOfDice = numberOfDice;
        SidesPerDie = sidesPerDie;
        CostPerRound = costPerRound;
        Type = type;
    }

    public int CalculateDamage()
    {
        Random r = new Random();
        int total = 0;

        for (int i = 0; i < NumberOfDice; i++)
        {
            total += r.Next(1, SidesPerDie);
        }

        return total;
    }
}