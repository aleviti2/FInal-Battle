
public interface IAction
{
    public string Name { get; set; }
    public int HPInflicted { get; set; }
    public int Hit(ICharacter characterHit);
}

public class Punch : IAction
{
    public string Name { get; set; } = "Punch";
    public int HPInflicted { get; set; } = 1;
    public int Hit(ICharacter characterHit)
    {
        int newHP =characterHit.HP - HPInflicted;
        Console.WriteLine($"{characterHit.Name} has lost {HPInflicted} health point.");
        return newHP;
    }

}

public class BoneCrunch : IAction
{
    public string Name { get; set; } = "BoneCrunch";
    public int HPInflicted { get; set; }
    public int Hit(ICharacter characterHit)
    {
        Random random = new Random();
        HPInflicted = random.Next(0, 2);
        int newHP = characterHit.HP - HPInflicted;
        Console.WriteLine($"{characterHit.Name}  has lost  {HPInflicted}  health point.");
        return newHP;
    }
}

public class Claw : IAction
{
    public string Name { get; set; } = "Claw";
    public int HPInflicted { get; set; } = 2;
    public int Hit(ICharacter characterHit)
    {
        int newHP = characterHit.HP - HPInflicted;
        Console.WriteLine($"{characterHit.Name} has lost {HPInflicted} health point.");
        return newHP;
    }
}

public class MistyFist : IAction
{
    public string Name { get; set; } = "MistyFist";
    public int HPInflicted { get; set; }
    public int Hit(ICharacter characterHit)
    {
        Random random = new Random();
        HPInflicted = random.Next(0, 3);
        int newHP = characterHit.HP - HPInflicted;
        Console.WriteLine($"{characterHit.Name}  has lost  {HPInflicted}  health point.");
        return newHP;
    }
}