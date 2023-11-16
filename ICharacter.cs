public interface ICharacter
{
    public int HP { get; set; }
    public string Name { get; }
    public int BattlesWon { get; set; }
    public List <AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
}

public abstract class Character : ICharacter
{
    public int HP { get; set; }
    public string Name { get; set; }
    public int BattlesWon { get; set; }
    public List<AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public Character ( int hp, int battlesWon, string name, Category category)
    {
        HP = hp;
        Name = name;
        BattlesWon= battlesWon;
        CharacterCategory= category;
    }
}

public class Hero : Character
{

    public CharacterType CharacterType { get; set; }
    public Hero(int hp, int battlesWon, string name, CharacterType cType) : base(hp, battlesWon, name, Category.Hero)
    {
        AttackT = new List<AttackType>
        {
            AttackType.Punch
        };
    }
    public string GiveName()
    {
        Console.WriteLine("Choose your battle name, valiant warrior, then press enter. If you have a companion, enter their name and press enter.");
        Name = Console.ReadLine();
        return Name;

    }
}

public class Skeleton : Character
{

    //public Category CharacterCategory { get; set; } = Category.Skeleton;

    public Skeleton(int hp, int battlesWon, string name) : base(hp, battlesWon, name, Category.Skeleton)
    {
        AttackT = new List<AttackType>
        {
            AttackType.BoneCrunch
        };
    }

}

public class Werewolf : Character
{
    public Werewolf(int hp, int battlesWon, string name) : base(hp, battlesWon, name, Category.Werewolf)
    {
        AttackT = new List<AttackType>
        {
            AttackType.Claw
        };
    }
}

public class TheUncodedOne : Character
{
    public TheUncodedOne(int hp, int battlesWon) : base(hp, battlesWon, "Mephisto", Category.TheUncodedOne)
    {
        AttackT = new List<AttackType>
        {
            AttackType.MistyFist
        };
    }
}