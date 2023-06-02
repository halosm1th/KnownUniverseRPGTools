namespace SimpleTester;

class SimpleKURPGCharacter
{
    public int Fort{ get; private set; }
    public int Ref{ get; private set; }
    public int Brawn { get; private set; }

    public int CurrentHP => Fort + Brawn + Ref;

    public bool IsAlive => Fort > 0 || Ref > 0 || Brawn > 0;
    
    public int DodgeSkill { get; }
    public int GunCombatMod { get; }
    public int MeleeCombatMod { get; }
    public SimpleKURPGWeapon Weapon { get; }
    public SimpleKURPGArmour Armour { get; }

    public string Name { get; }

    public SimpleKURPGCharacter(int fort, int @ref, int brawn, int dodgeSkill, int gunCombatMod, int meleeCombatMod, SimpleKURPGWeapon weapon, SimpleKURPGArmour armour, string name)
    {
        Fort = fort;
        Ref = @ref;
        Brawn = brawn;
        DodgeSkill = dodgeSkill;
        GunCombatMod = gunCombatMod;
        MeleeCombatMod = meleeCombatMod;
        Weapon = weapon;
        Armour = armour;
        Name = name;
    }


    public void ApplyDamage(int damage)
    {
        if (Fort > 0)
        {
            if (Fort - damage < 0)
            {
                damage -= Fort;
                Fort = 0;
            }
            else
            {
                Fort -= damage;
                return;
            }
        }

        if (damage > 0 && Ref > 0)
        {
            if (Ref - damage < 0)
            {
                damage -= Ref;
                Ref = 0;
            }
            else
            {
                Ref = -damage;
                return;
            }
        }

        if (damage > 0 && Brawn > 0)
        {
            if (Brawn - damage < 0)
            {
                damage -= Brawn;
                Brawn = 0;
            }
            else
            {
                Brawn -= damage;
                return;
            }
        }
    }

    public int FortMod => Fort switch
    {
        0 => -3,
        1 => -2,
        2 => -2,
        3 => -1,
        4 => -1,
        5 => -1,
        6 => 0,
        7 => 0,
        8 => 0,
        9 => 1,
        10 => 1,
        11 => 1,
        12 => 2,
        13 => 2,
        14 => 2,
        _ => -4,
    };
    
    public int RefMod => Ref switch
    {
        0 => -3,
        1 => -2,
        2 => -2,
        3 => -1,
        4 => -1,
        5 => -1,
        6 => 0,
        7 => 0,
        8 => 0,
        9 => 1,
        10 => 1,
        11 => 1,
        12 => 2,
        13 => 2,
        14 => 2,
        _ => -4,
    };
    
    public int BrawnMod => Brawn switch
    {
        0 => -3,
        1 => -2,
        2 => -2,
        3 => -1,
        4 => -1,
        5 => -1,
        6 => 0,
        7 => 0,
        8 => 0,
        9 => 1,
        10 => 1,
        11 => 1,
        12 => 2,
        13 => 2,
        14 => 2,
        _ => -4,
    };
}