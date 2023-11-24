using System.Threading;

public class AttackModifier
{
    public AttackModifierEnum Name { get; set; }
    public int HPSaved { get; set; }
    public int HitsBeforeBreaking { get; set; }
    //public ThunderBlast ThunderBlast { get; set; }
    //public Annihilator Annihilator { get; set; }
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
    public void CheckForAttackModifier(ICharacter attacker, ICharacter receiver) 
    {
        if (receiver.AttackModifier != null && receiver.HitsTakenPerBattle <= HitsBeforeBreaking)
        {
            AttackMod(attacker, receiver);
        }
        if (receiver.AttackModifier != null && receiver.HitsTakenPerBattle == HitsBeforeBreaking + 1)
        {

            Console.WriteLine($"{receiver.Name}'s shield is broken.");
            Console.WriteLine($"{receiver.Name}'s HP is now {receiver.HP}");
        }
        else
            Console.WriteLine($"{receiver.Name}'s HP is now {receiver.HP}");
    }

    public void BuyActions(BattleSeries battleseries, List<ICharacter> heroes, GameEngine gameEngine)
    {
        //ThunderBlast thunderBlast = new ThunderBlast(3);
        //ThunderBlast = thunderBlast;
        BuyAction(battleseries, heroes, gameEngine.ThunderBlast);
        //Annihilator annihilator = new Annihilator(4, 4);
        //Annihilator = annihilator;
        BuyAction(battleseries, heroes, gameEngine.Annihilator);
    }

    private void BuyAction(BattleSeries battleseries, List<ICharacter> heroes, IExtraActions extraAction)
    {
        if (battleseries.CurrentBattleNumber == extraAction.RequiredBattleNumber)
        {
            Console.WriteLine($"The gods are bestowing upon you a new power: {extraAction.Name}, capable of causing {extraAction.HPInflicted} HP damage to your opponent.");
            Console.WriteLine($"When you use it, this power could backfire, which could result in a loss of {extraAction.MAXHPSelfInflicted} HP.");
            if (extraAction.Name == "Annihilator")
                Console.WriteLine($"This deadly power comes with an added steep price: by using it, you have 1 chance out of {extraAction.ChanceOfDying} of dying.");
            Console.WriteLine($"Also, this will come with a price. If you accept it, you will have to sacrifice {extraAction.Cost} HP. Will you take it? Yes/No.");
           

            string input = Console.ReadLine();

            if (input != null && input == "Yes" && Enum.TryParse<AttackType>(extraAction.Name, out AttackType attackType))
            {
                foreach (ICharacter hero in heroes)
                {
                    if (hero.HP > extraAction.Cost)
                    {
                        hero.AttackT.Add(attackType);
                        hero.HP -= extraAction.Cost;
                        Console.WriteLine($"{hero.Name} acquired {extraAction.Name}.Their new HP is now {hero.HP}");
                    }
                    else /*if (hero.HP <= extraAction.Cost)*/
                    {
                        Console.WriteLine($"{hero.Name} doens't have enough HP to acquire the {extraAction.Name} power.");
                    }
                }                                                   
            }
            else if (input != null && input == "No")
            {
                Console.WriteLine("You have decided to refuse.");
            }
        }
    }

    //public void BuyActions(BattleSeries battleseries, List<ICharacter> heroes)
    //{
    //    ThunderBlast thunderBlast = new ThunderBlast(3);
    //    Annihilator annihilator = new Annihilator(4);
    //    if(battleseries.CurrentBattleNumber == 2)
    //    {           
    //        Console.WriteLine($"The gods are bestowing upon you a new power: {thunderBlast.Name}, capable of causing {thunderBlast.HPInflicted} HP damage to your opponent.");
    //        Console.WriteLine($"This will come with a price. If you accept it, you will have to sacrifice {thunderBlast.Cost} HP. Will you take it? Yes/No.");
    //        Console.WriteLine($"Also, when you use it, there's one chance over five that it backfires, cwhich will result in a loss of {thunderBlast.MAXHPSelfInflicted} HP.");
    //        string input = Console.ReadLine();
    //        if (input != null && input == "Yes")
    //        {
    //            foreach (ICharacter hero in heroes)
    //            {
    //                hero.AttackT.Add(AttackType.ThunderBlast);
    //                Console.WriteLine($"{thunderBlast.Name} equipped.");
    //            }
    //        }
    //        if (input != null && input == "No")
    //        {
    //            Console.WriteLine("You have decided to refuse.");
    //        }
    //    }
    //    else if (battleseries.CurrentBattleNumber == 3)
    //    {
    //        Console.WriteLine($"The gods are bestowing upon you a new power: {annihilator.Name}, capable of causing {annihilator.HPInflicted} HP damage to your opponent.");
    //        Console.WriteLine($"This will come with a price. If you accept it, you will have to sacrifice {annihilator.Cost} HP. Will you take it? Yes/No.");
    //        Console.WriteLine($"Also, when you use it, there's one chance over five that it backfires, cwhich will result in a loss of {annihilator.MAXHPSelfInflicted} HP.");
    //        string input = Console.ReadLine();
    //        if (input != null && input == "Yes")
    //        {
    //            foreach (ICharacter hero in heroes)
    //            {
    //                hero.AttackT.Add(AttackType.ThunderBlast);
    //                Console.WriteLine($"{annihilator.Name} equipped.");
    //            }
    //        }
    //        if (input != null && input == "No")
    //        {
    //            Console.WriteLine("You have decided to refuse.");
    //        }
    //    }

    //}
}