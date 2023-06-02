using System.Security.Cryptography;

namespace KnownUniverseCombatTester;

public interface IKUCombatUI
{
    public void ShowText(string text);
    public string AskQuestion(string questionText);
    public string MakeChoice(string questionText, List<string> Choices);
}

public class KUCombatConsole : IKUCombatUI
{
    public void ShowText(string text)
    {
        Console.WriteLine(text);
    }

    public string AskQuestion(string questionText)
    {
        Console.WriteLine(questionText);
        return Console.ReadLine() ?? AskQuestion(questionText);
    }

    public string MakeChoice(string questionText, List<string> Choices)
    {
        int choiceNumber = 0;
        
        Console.WriteLine(questionText);
        
        for (int i = 0; i < Choices.Count(); i++)
        {
            Console.WriteLine($"{i}) {Choices[i]}");
        }

        choiceNumber = GetInt();

        return Choices[choiceNumber];
    }

    public int GetInt()
    {
        Console.Write("Enter Number: ");
        var numberText = Console.ReadLine();

        if (numberText != null)
        {
            if (int.TryParse(numberText, out int numb))
            {
                return numb;
            }
        }

        return GetInt();
    }
}

public class KUCombat
{
    private List<KUCharacter> Combatants { get; }
    private int rounds = 0;

    public void Setup(IKUCombatUI UI)
    {
        
    }

    public void GunAttack(KUCharacter attacker, KUCharacter defender)
    {
        var ToHitMod = attacker.GunCombat + attacker.Reflex;
    }

    public void CombatTest()
    {
        
    }
}