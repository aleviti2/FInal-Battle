public class Skeleton : ICharacter
{
    public int HP { get; set; }
    public int BattlesWon { get; set; }
    public string Name { get; set; }
    public List <AttackType> AttackT { get; set; }
    public Category CharacterCategory { get; } = Category.Skeleton;
    public bool IsDead { get; set; }
    public bool Turn { get; set; }
    public Skeleton (int hp, int battlesWon)
    {
        HP = hp;
        BattlesWon = battlesWon;
        Name = "Skelly";
        AttackT = new List<AttackType>
        {
            AttackType.BoneCrunch
        };
    }

}
