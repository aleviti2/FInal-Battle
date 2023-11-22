public class AttackModifier
{
    public AttackModifierEnum Name { get; set; }
    public int HPSaved { get; set; }
    public int HitsBeforeBreaking { get; set; }
    public AttackModifier (AttackModifierEnum name, int hpSaved, int hitsBeforeBreaking)
    {
        Name = name;
        HPSaved = hpSaved; 
        HitsBeforeBreaking = hitsBeforeBreaking;
    }
    public void AttackMod(ICharacter attacker, ICharacter receiver)
    {
        int originalHP = receiver.HP;
        int newHP = receiver.HP + HPSaved;
        if (newHP <= receiver.MaxHP)
        {
            receiver.HP = newHP;
            Console.WriteLine($"However, {receiver.Name} is wearing a {Name}, so they regained {HPSaved} HP. Their new HP is {receiver.HP}");
        }
        else
        {
            receiver.HP = receiver.MaxHP;
            Console.WriteLine($"{receiver.Name} is wearing a {Name}, which restored them to full health. Their HP is now maxed out: ({receiver.HP}/{receiver.MaxHP})");
        }     
    }
}