public class Werewolf : ICharacter
{
    public int HP { get; set; }
    public int BattlesWon { get; set; }
    public string Name { get; set; }
    public List<AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; } = Category.Werewolf;
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public Werewolf(int hp, string name)
    {
        HP = hp;
        Name = name;
        AttackT = new List<AttackType>
        {
            AttackType.Claw
        };
    }
}
