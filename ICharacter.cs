public interface ICharacter
{
    public int HP { get; set; }
    public int MaxHP { get; init; }
    public string Name { get; }
    public int BattlesWon { get; set; }
    public List <AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public int PotionsAvailable { get; set; }
    public AttackModifier AttackModifier { get; set; }
    public int HitsTakenPerBattle { get; set; }
}

public abstract class Character : ICharacter
{
    public int HP { get; set; }
    public int MaxHP { get; init; }
    public string Name { get; set; }
    public int BattlesWon { get; set; }
    public List<AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public int PotionsAvailable { get; set; }
    public AttackModifier AttackModifier { get; set; }
    public int HitsTakenPerBattle { get; set; } = 0;
    public Character ( int hp, int maxHP, int potionsAvailable, string name, Category category)
    {
        HP = hp;
        MaxHP = maxHP;
        Name = name;
        PotionsAvailable= potionsAvailable;
        CharacterCategory= category;
    }
}

public class Hero : Character
{

    public CharacterType CharacterType { get; set; }
    public Hero(int hp, int maxHP, int battlesWon, string name, CharacterType cType) : base(hp, maxHP, battlesWon, name, Category.Hero)
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

    public Skeleton(int hp, int maxHP, int potionsAvailable, string name) : base(hp, maxHP, potionsAvailable, name, Category.Skeleton)
    {
        AttackT = new List<AttackType>
        {
            AttackType.BoneCrunch
        };
    }

}

public class Werewolf : Character
{
    public Werewolf(int hp, int maxHP, int potionsAvailable, string name) : base(hp, maxHP, potionsAvailable, name, Category.Werewolf)
    {
        AttackT = new List<AttackType>
        {
            AttackType.Claw
        };
    }
}

public class TheUncodedOne : Character
{
    public TheUncodedOne(int hp, int maxHP, int potionsAvailable) : base(hp, maxHP, potionsAvailable, "Mephisto", Category.TheUncodedOne)
    {
        AttackT = new List<AttackType>
        {
            AttackType.MistyFist
        };
    }
}