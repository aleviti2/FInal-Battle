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
