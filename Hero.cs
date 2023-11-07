public class Hero : ICharacter
{
    public string Name { get; private set; }
    public CharacterType CharacterType { get; set; }
    public int HP { get; set; }
    public List<AttackType> AttackT { get; set; }
    public int BattlesWon { get; set; }
    public Category CharacterCategory { get; set; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public Hero(int hp, int battlesWon, CharacterType characterType)
    {
        HP = hp;
        BattlesWon = battlesWon;
        CharacterType = characterType;

        AttackT = new List<AttackType>
        {
            AttackType.Punch
        };
    }
    public string GiveName()
    {
        Console.WriteLine("Choose your battle name, valiant warrior, then press enter. If you have a companion, enter their name and press enter.");
        string name = Console.ReadLine();
        this.Name = name;
        return this.Name;

    }
}
