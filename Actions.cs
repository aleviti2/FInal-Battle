
using System;

public interface IAction
{
    public string Name { get; }
    public int HPInflicted { get; }
    //public int RequiredBattleNumber { get; }

    public int Hit(ICharacter characterHit, ICharacter attacker);
}

public class Punch : IAction
{
    public string Name { get; } = "Punch";
    public int HPInflicted { get; } = 5;
    public int RequiredBattleNumber { get; set; }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        int newHP =characterHit.HP - HPInflicted;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{attacker.Name} has landed a Punch, dealing {HPInflicted} points of damage to {characterHit}'s health.");
        Console.ResetColor();
        return newHP;
    }

}

public class BoneCrunch : IAction
{
    public string Name { get; } = "BoneCrunch";
    public int HPInflicted { get; set; }
    public int RequiredBattleNumber { get; set; }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        Random random = new Random();
        HPInflicted = random.Next(3, 11);
        int newHP = characterHit.HP - HPInflicted;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{attacker.Name} has landed a Bone Crunch, dealing {HPInflicted} points of damage to {characterHit}'s health.");
        Console.ResetColor();
        return newHP;
    }
}

public class Claw : IAction
{
    public string Name { get; } = "Claw";
    public int HPInflicted { get; set; } 
    public int RequiredBattleNumber { get; set; }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        Random random = new Random();
        HPInflicted = random.Next(5, 15);
        int newHP = characterHit.HP - HPInflicted;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{attacker.Name} has landed a Claw, dealing {HPInflicted} points of damage to {characterHit}'s health.");
        Console.ResetColor();
        return newHP;
        //int newHP = characterHit.HP - HPInflicted;
        //Console.ForegroundColor = ConsoleColor.DarkRed;
        //Console.WriteLine($"{attacker.Name} has landed a Claw, dealing {HPInflicted} points of damage to {characterHit}'s health.");
        //Console.ResetColor();
        //return newHP;
    }
}

public class MistyFist : IAction
{
    public string Name { get; } = "MistyFist";
    public int HPInflicted { get; set; }
    public int RequiredBattleNumber { get; set; }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        Random random = new Random();
        HPInflicted = random.Next(7, 18);
        int newHP = characterHit.HP - HPInflicted;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{attacker.Name} has landed a Misty Fist, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
        Console.ResetColor();
        return newHP;
    }
}

public class HealthPotion : IAction
{
    public string Name { get; } = "Potion";
    public int HPInflicted { get; }
    public int RequiredBattleNumber { get; set; }
    public HealthPotion(int hpGained)
    { 
        HPInflicted = hpGained;
    }
    public int Hit(ICharacter potionOwner, ICharacter attacker)
    {
        if ((potionOwner.PotionsAvailable > 0) && (potionOwner.HP < potionOwner.MaxHP))
        {
            int nwHP = potionOwner.HP + HPInflicted;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"The potion made {potionOwner.Name} regain {HPInflicted} health points!");
            Console.ResetColor();
            potionOwner.PotionsAvailable--;
            return nwHP;
        }
        if ((potionOwner.PotionsAvailable > 0) && (potionOwner.HP == potionOwner.MaxHP))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Your HP are already maxed out. No further increase is possible.");
            Console.ResetColor();
            return potionOwner.HP;
        }
        else 
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You don't have any potion available");
            Console.ResetColor();
            return potionOwner.HP; 
        }
    }
}
public interface IExtraActions
{
    public string Name { get; }
    public int HPInflicted { get; }
    public int MAXHPSelfInflicted { get;  }
    public int Cost { get; }
    public int RequiredBattleNumber { get;} 
    public int ChanceOfDying { get; set; }
}
public class ThunderBlast : IExtraActions
{
    public string Name { get; } = "ThunderBlast";
    public int HPInflicted { get; } = 9;            //Points inflicted to victim
    public int MAXHPSelfInflicted { get; set; }     //Points inflicted to attacker. Add 1 to the value when creating an instance of the object
    public int Cost { get; } = 2;
    public int RequiredBattleNumber { get; } = 2;
    public int ChanceOfDying {  set; get; }
    public int SelfInflicted { set; get; }
    public ThunderBlast(int maxHPSelfInflicted)
    {
        MAXHPSelfInflicted = maxHPSelfInflicted;
    }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        Random random = new Random();
        SelfInflicted = random.Next(0, MAXHPSelfInflicted);
        int newHP = characterHit.HP - HPInflicted;
        attacker.HP -= SelfInflicted;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"{attacker.Name} has landed a ThunderBlast, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
        Console.WriteLine($"{attacker.Name} has suffered {SelfInflicted} HP. Their new HP is {attacker.HP}");
        Console.ResetColor();
        return newHP;
    }
}

public class Annihilator : IExtraActions
{
    public string Name { get; } = "Annihilator";
    public int HPInflicted { get; } = 15;
    public int MAXHPSelfInflicted { get; set; }
    public int Cost { get; } = 3;
    public int RequiredBattleNumber { get; set; } = 3;
    public int ChanceOfDying { get; set; }
    public int SelfInflicted { get; set; }
    public Annihilator(int  maxHPSelfInflicted, int chanceOfDying)
    {
        MAXHPSelfInflicted = maxHPSelfInflicted;
        ChanceOfDying = chanceOfDying;
    }
    public int Hit(ICharacter characterHit, ICharacter attacker)
    {
        Random rand = new Random();
        int willIDie = rand.Next(1, ChanceOfDying);
        if (willIDie == 1) 
        {
            characterHit.HP -= HPInflicted;
            attacker.HP = 0;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{attacker.Name} has landed an Annihilator, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
            Console.WriteLine($"{Name} was too powerful for {attacker.Name}.");
            Console.ResetColor();
        }
        else
        {
            SelfInflicted = rand.Next(1, MAXHPSelfInflicted);
            attacker.HP -= SelfInflicted;
            characterHit.HP -= HPInflicted;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{attacker.Name} has landed an Annihilator, dealing {HPInflicted} points of damage to {characterHit.Name}'s health.");
            Console.WriteLine($"{attacker.Name} has suffered {SelfInflicted} HP. Their new HP is {attacker.HP}");
            Console.ResetColor();
        }
        return characterHit.HP;
    }
}

