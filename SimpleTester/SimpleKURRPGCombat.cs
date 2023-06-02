using Dice;
using KnownUniverseCombatTester;

namespace SimpleTester;

class SimpleKURRPGCombat
{
    private SimpleKURPGCharacter Character1;
    private SimpleKURPGCharacter Character2;

    private bool Character1WonInitative = false;
    private KUWeaponRange RangeOfCombat;

    private readonly int TargetNumber = 9;
    
    public void Init(SimpleKURPGCharacter c1, SimpleKURPGCharacter c2, KUWeaponRange rangeOfCombat)
    {
        Character1 = c1;
        Character2 = c2;
        RangeOfCombat = rangeOfCombat;
        
        Character1WonInitative = false;

        Console.WriteLine($"Initing new combat with: {c1.Name} ({c1.Weapon.Name}, {c1.Armour.Name}) " +
                          $"& {c2.Name} ({c2.Weapon.Name}, {c2.Armour.Name})");
    }

    public bool RunCombat()
    {
        RollInitative();
        if(Character1WonInitative) Console.WriteLine($"{Character1.Name} won the initative");
        else Console.WriteLine($"{Character2.Name} won the initative");

        int round = 0;
        while(Character1.IsAlive && Character2.IsAlive)
        {
            Console.WriteLine($"Round #{round}. {Character1.Name} (HP: {Character1.CurrentHP}) VS {Character2.Name} (HP: {Character2.CurrentHP}) ");

            if (Character1WonInitative)
            {
                Attack(Character1, Character2);
                Attack(Character2, Character1);
            }
            else
            {
                Attack(Character2, Character1);
                Attack(Character1, Character2);
            }

            //Console.WriteLine("Press any key for the next round");
            //Console.ReadKey();
            round++;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        
        if (Character1.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{Character1.Name} Won!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Character2.Name} Won!");
        }
        Console.ForegroundColor = ConsoleColor.Gray;

        return Character1.IsAlive;
        
    }

    private void Attack(SimpleKURPGCharacter attacker, SimpleKURPGCharacter defender)
    {
        if (attacker.IsAlive)
        {

            for (int att = 0; att < attacker.Weapon.RateOfFire; att++)
            {
                if (attacker.Weapon.RangeOrMelee == KURangeOrMelee.Ranged)
                {
                    RangedAttack(attacker, defender);
                }
                else
                {
                    MeleeAttack(attacker, defender);
                }
            }
        }
    }

    private void RangedAttack(SimpleKURPGCharacter attacker, SimpleKURPGCharacter defender)
    {
        var RangeMod = GetRangeMod(attacker.Weapon.Range);
        var CharacterMod = attacker.RefMod + attacker.GunCombatMod;
        var totalMod = RangeMod + CharacterMod;
        var roll = Roll(attacker.Weapon) + totalMod;

        if (roll >= TargetNumber)
        {
            Console.WriteLine($"{attacker.Name} attempts a ranged attack with their {attacker.Weapon.Name}, rolling: {roll} vs {TargetNumber}, Succeeding!");
            var effect = roll - TargetNumber;
            var damage = Roll(attacker.Weapon.DamageDice) +attacker.Weapon.BonusDamage + effect;
            ApplyDamage(damage, attacker.Weapon.DamageType, defender);
        }
        else
        {
            Console.WriteLine($"{attacker.Name} attempts a ranged attack with their {attacker.Weapon.Name}, rolling: {roll} vs {TargetNumber}, failing.");
            
        }
    }

    public int ArmourStoppingPowerBasedOnDamageType(KUWeaponType attackDamageType, SimpleKURPGArmour defender)
    {
        return defender.DamageType switch
        {
            KUWeaponType.DirectEnergy => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP ,
                KUWeaponType.LaserEnergy => defender.SP ,
                KUWeaponType.Slug => defender.SP / 2,
                KUWeaponType.Archeric => defender.SP /2 ,
                KUWeaponType.Plasma => defender.SP / 2,
                KUWeaponType.Void => defender.SP ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.LaserEnergy => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP ,
                KUWeaponType.LaserEnergy => defender.SP ,
                KUWeaponType.Slug => defender.SP / 2,
                KUWeaponType.Archeric => defender.SP /2 ,
                KUWeaponType.Plasma => defender.SP / 2,
                KUWeaponType.Void => defender.SP ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Slug => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP / 2,
                KUWeaponType.LaserEnergy => defender.SP / 2,
                KUWeaponType.Slug => defender.SP ,
                KUWeaponType.Archeric => defender.SP * 2 ,
                KUWeaponType.Plasma => defender.SP / 2,
                KUWeaponType.Void => defender.SP ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Archeric => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP / 2,
                KUWeaponType.LaserEnergy => defender.SP / 2,
                KUWeaponType.Slug => defender.SP,
                KUWeaponType.Archeric => defender.SP ,
                KUWeaponType.Plasma => defender.SP / 2,
                KUWeaponType.Void => defender.SP / 2,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Plasma => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP * 2,
                KUWeaponType.LaserEnergy => defender.SP * 2,
                KUWeaponType.Slug => defender.SP / 2,
                KUWeaponType.Archeric => defender.SP  ,
                KUWeaponType.Plasma => defender.SP ,
                KUWeaponType.Void => defender.SP ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Void => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => defender.SP ,
                KUWeaponType.LaserEnergy => defender.SP ,
                KUWeaponType.Slug => defender.SP ,
                KUWeaponType.Archeric => defender.SP  ,
                KUWeaponType.Plasma => defender.SP,
                KUWeaponType.Void => defender.SP ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public string ArmourStoppingPowerText(KUWeaponType attackDamageType, SimpleKURPGArmour defender)
    {
        return defender.DamageType switch
        {
            KUWeaponType.DirectEnergy => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "Armour offers no extra or reduced protection.",
                KUWeaponType.LaserEnergy => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Slug => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Archeric =>"this armour is less effective against this damage type cutting its protection in half." ,
                KUWeaponType.Plasma => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Void => "Armour offers no extra or reduced protection." ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.LaserEnergy => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "Armour offers no extra or reduced protection." ,
                KUWeaponType.LaserEnergy => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Slug => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Archeric => "this armour is less effective against this damage type cutting its protection in half." ,
                KUWeaponType.Plasma => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Void => "Armour offers no extra or reduced protection." ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Slug => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.LaserEnergy => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Slug => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Archeric => "Armour is more effective against this damage type doubling its protection." ,
                KUWeaponType.Plasma => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Void => "Armour offers no extra or reduced protection." ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Archeric => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.LaserEnergy => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Slug => "Armour offers no extra or reduced protection.",
                KUWeaponType.Archeric => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Plasma => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Void => "this armour is less effective against this damage type cutting its protection in half.",
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Plasma => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "Armour is more effective against this damage type doubling its protection." ,
                KUWeaponType.LaserEnergy => "Armour is more effective against this damage type doubling its protection." ,
                KUWeaponType.Slug => "this armour is less effective against this damage type cutting its protection in half.",
                KUWeaponType.Archeric => "Armour offers no extra or reduced protection."  ,
                KUWeaponType.Plasma => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Void => "Armour offers no extra or reduced protection." ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            KUWeaponType.Void => attackDamageType switch
            {
                KUWeaponType.DirectEnergy => "Armour offers no extra or reduced protection." ,
                KUWeaponType.LaserEnergy => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Slug => "Armour offers no extra or reduced protection." ,
                KUWeaponType.Archeric => "Armour offers no extra or reduced protection."  ,
                KUWeaponType.Plasma => "Armour offers no extra or reduced protection.",
                KUWeaponType.Void => "Armour offers no extra or reduced protection." ,
                _ => throw new ArgumentOutOfRangeException(nameof(attackDamageType), attackDamageType, null)
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void ApplyDamage(int damage, KUWeaponType attackDamageType, SimpleKURPGCharacter defender)
    {
        var armourStoppingPower = ArmourStoppingPowerBasedOnDamageType(attackDamageType, defender.Armour);
        var reducedDamage = damage - armourStoppingPower;
        if (reducedDamage > 0)
        {
            Console.WriteLine($"{defender.Name} suffers {damage} damage, but their {defender.Armour.Name} Armour blocks {armourStoppingPower}. {ArmourStoppingPowerText(attackDamageType,defender.Armour)}  Only {reducedDamage} gets through, and the armours SP is reduced to {defender.Armour.SP-1}");
            defender.Armour.ReduceSP();
            defender.ApplyDamage(reducedDamage);
            Console.WriteLine($"{defender.Name} is down to {defender.CurrentHP} HP.");
        }
        else
        {
            Console.WriteLine($"The attack did {damage} damage, but {defender.Name}'s {defender.Armour.Name} Armour (sp: {armourStoppingPower}) absorbed the hit.");
        }
    }
    
    public int GetRangeMod(KUWeaponRange weaponRange)
    {
        switch(weaponRange) 
        {
            case KUWeaponRange.Melee:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => 0,
                    KUWeaponRange.Point_Blank => -1,
                    KUWeaponRange.Close => -2,
                    KUWeaponRange.Short => -3,
                    KUWeaponRange.Average => -4,
                    KUWeaponRange.Long => -5,
                    KUWeaponRange.Far => -6,
                    KUWeaponRange.Extreme => -7,
                    KUWeaponRange.Impossible => -8,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case KUWeaponRange.Point_Blank:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => 1,
                    KUWeaponRange.Point_Blank => 0,
                    KUWeaponRange.Close => -1,
                    KUWeaponRange.Short => -2,
                    KUWeaponRange.Average => -3,
                    KUWeaponRange.Long => -4,
                    KUWeaponRange.Far => -5,
                    KUWeaponRange.Extreme => -6,
                    KUWeaponRange.Impossible => -7,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
            case KUWeaponRange.Close:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => 2,
                    KUWeaponRange.Point_Blank => 1,
                    KUWeaponRange.Close => 0,
                    KUWeaponRange.Short => -1,
                    KUWeaponRange.Average => -2,
                    KUWeaponRange.Long => -3,
                    KUWeaponRange.Far => -4,
                    KUWeaponRange.Extreme => -5,
                    KUWeaponRange.Impossible => -6,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case KUWeaponRange.Short:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => 3,
                    KUWeaponRange.Point_Blank => 2,
                    KUWeaponRange.Close => 1,
                    KUWeaponRange.Short => 0,
                    KUWeaponRange.Average => -1,
                    KUWeaponRange.Long => -2,
                    KUWeaponRange.Far => -3,
                    KUWeaponRange.Extreme => -4,
                    KUWeaponRange.Impossible => -5,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
            case KUWeaponRange.Average:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => -1,
                    KUWeaponRange.Point_Blank => 3,
                    KUWeaponRange.Close => 2,
                    KUWeaponRange.Short => 1,
                    KUWeaponRange.Average => 0,
                    KUWeaponRange.Long => -1,
                    KUWeaponRange.Far => -2,
                    KUWeaponRange.Extreme => -3,
                    KUWeaponRange.Impossible => -4,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case KUWeaponRange.Long:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => -2,
                    KUWeaponRange.Point_Blank => -1,
                    KUWeaponRange.Close => 3,
                    KUWeaponRange.Short => 2,
                    KUWeaponRange.Average => 1,
                    KUWeaponRange.Long => 0,
                    KUWeaponRange.Far => -1,
                    KUWeaponRange.Extreme => -2,
                    KUWeaponRange.Impossible => -3,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case KUWeaponRange.Far:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => -3,
                    KUWeaponRange.Point_Blank => -2,
                    KUWeaponRange.Close => -1,
                    KUWeaponRange.Short => 3,
                    KUWeaponRange.Average => 2,
                    KUWeaponRange.Long => 1,
                    KUWeaponRange.Far => 0,
                    KUWeaponRange.Extreme => -1,
                    KUWeaponRange.Impossible => -2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case KUWeaponRange.Extreme:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => -4,
                    KUWeaponRange.Point_Blank => -3,
                    KUWeaponRange.Close => -2,
                    KUWeaponRange.Short => -1,
                    KUWeaponRange.Average => 3,
                    KUWeaponRange.Long => 2,
                    KUWeaponRange.Far => 1,
                    KUWeaponRange.Extreme => 0,
                    KUWeaponRange.Impossible => -1,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
            case KUWeaponRange.Impossible:
                return RangeOfCombat switch
                {
                    KUWeaponRange.Melee => -5,
                    KUWeaponRange.Point_Blank => -4,
                    KUWeaponRange.Close => -3,
                    KUWeaponRange.Short => -2,
                    KUWeaponRange.Average => -1,
                    KUWeaponRange.Long => 3,
                    KUWeaponRange.Far => 2,
                    KUWeaponRange.Extreme => 1,
                    KUWeaponRange.Impossible => 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weaponRange), weaponRange, null);
        }
    }
    
    private void MeleeAttack(SimpleKURPGCharacter attacker, SimpleKURPGCharacter defender)
    {
        var AttackerMods = attacker.BrawnMod + attacker.MeleeCombatMod;
        var DefenderMods = defender.RefMod + defender.DodgeSkill;

        var attackRoll = Roll("2d6") + AttackerMods;
        var defendRoll = Roll("2d6") + DefenderMods;

        if (attackRoll > defendRoll)
        {
            Console.WriteLine($"{attacker.Name} attempts a melee attack while {defender.Name} tries to dodge, rolling: {attackRoll} vs {defendRoll}. {attacker.Name} Hits!");
            var effect = attackRoll - defendRoll; 
            var damage = Roll(attacker.Weapon)
                         +attacker.Weapon.BonusDamage + effect;

            ApplyDamage(damage,attacker.Weapon.DamageType, defender);
        }
        else
        {
            Console.WriteLine($"{attacker.Name} attempts a melee attack while {defender.Name} tries to dodge, rolling: {attackRoll} vs {defendRoll}. {defender.Name} Dodges!");
        }
    }


    private int Roll(SimpleKURPGWeapon weapon)
    {
        if (weapon.Quality == KUWeaponQuality.Good)
        {
            return Roll(weapon.DamageDice + "+1");
        }else if (weapon.Quality == KUWeaponQuality.Military)
        {
            return Roll(weapon.DamageDice + "dl1");
        }else if (weapon.Quality == KUWeaponQuality.Spec_Ops)
        {
            return Roll(weapon.DamageDice + "dl1+1");
        }
        else
        {
            return Roll(weapon.DamageDice);
        }
    }

    private RollResult RollExpression(string roll = "2d6")
    {
        return Roller.Roll(roll);
    }
    
    private int Roll(string roll = "2d6")
    {
        return Convert.ToInt32(Roller.Roll(roll).Value);
    }

    private void RollInitative()
    {
        var c1Roll = Roll() + Character1.RefMod;
        var c2Roll = Roll() + Character2.RefMod;

        Character1WonInitative = c1Roll > c2Roll;
    }
}