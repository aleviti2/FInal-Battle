public class Hero : ICharacter
{
    public string Name { get; set; }
    public CharacterType CharacterType { get; set; }
    public int HP { get; set; }
    public List<AttackType> AttackT { get; set; }
    public int BattlesWon { get; set; }
    public Category CharacterCategory { get; set; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public Hero(int hp, int battlesWon, CharacterType characterType, string name)
    {
        HP = hp;
        BattlesWon = battlesWon;
        CharacterType = characterType;
        Name = name;

        AttackT = new List<AttackType>
        {
            AttackType.Punch
        };
    }
    public string GiveName(Party heroesParty)
    {

        while (heroesParty.AllCharactersList.Any())
        {
            Console.WriteLine("Choose your battle name, valiant warrior, then press enter. If you have a companion, enter their name and press enter.");
            foreach (Hero hero in heroesParty)
            {
                this.Name = Console.ReadLine();
                if (!string.IsNullOrEmpty(this.Name))
                {
                    return this.Name;
                }
            }

        }

        Console.WriteLine("The name you provided is not valid.");
        return this.GiveName(heroesParty);
    }
}
