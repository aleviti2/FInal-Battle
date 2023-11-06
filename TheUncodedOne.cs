public class TheUncodedOne : ICharacter
{
    public int HP { get; set; } = 10;
    public string Name { get; set; } = "Mephisto";
    public int BattlesWon { get; set; }
    public List<AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; set; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public TheUncodedOne(int hp, int battlesWon)
    {
        HP = hp;
        BattlesWon = battlesWon;
        
        AttackT = new List<AttackType>
        {
            AttackType.MistyFist
        };
    }
}
