
public interface IAction
{
    public int HPInflicted { get; set; }
    public int Hit(ICharacter characterHit);
}

public class Punch : IAction
{
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