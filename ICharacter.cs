public interface ICharacter
{
    public int HP { get; set; }
    public string Name { get; set; }
    public int BattlesWon { get; set; }
    public List <AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; set; }
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
}
