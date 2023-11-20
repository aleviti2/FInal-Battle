using System.Collections;
using System.Collections.Generic;

public class Party : IEnumerable<ICharacter>
{
    public List<ICharacter> AllCharactersList { get; set; }
    public List<ICharacter> HeroesParty { get; set; }
    public List<ICharacter> MonstersParty { get; set; }  
    public GameEngine TurnList { get; set; }


    public Party()
    {
        AllCharactersList = new List<ICharacter>();
        HeroesParty = new List<ICharacter>();
        MonstersParty = new List<ICharacter>();
    }
  
    public void AddCharacter(ICharacter ch)
    {

        if (ch is Hero hero)
        {
            HeroesParty.Add(ch);
        }
        else
        {
            MonstersParty.Add(ch);
        }      
    }
    public List<ICharacter> MergeLists(List<ICharacter> heroesParty, List<ICharacter> monstersParty)
    {
        AllCharactersList = heroesParty.Concat(monstersParty).ToList();
        return AllCharactersList;
    }

    public IEnumerator<ICharacter> GetEnumerator()
    {
        return AllCharactersList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void AIMonsters(ICharacter monster)
    {
        Random random = new Random();
        Console.WriteLine($"It's {monster.Name}'s turn. Their health points are {monster.HP}");
        Thread.Sleep(1000);
        Console.WriteLine($"{monster.Name} is getting ready to strike.");
        Thread.Sleep(3000);
        List<ICharacter> weakestList = (from o in HeroesParty
                                        where !o.IsDead
                                        orderby o.HP
                                        select o).ToList();
        ICharacter randomTarget = weakestList[random.Next(weakestList.Count)];
        //ICharacter randomTarget = weakestList[0];
        List <AttackType> availableAttacks =    (from o in monster.AttackT
                                                select o).ToList();
        List<IAction> listOfActions = new List<IAction>() { new Punch(), new BoneCrunch(), new Claw(), new MistyFist() };
        List<IAction> listOfavailableActions= new List<IAction>();
        foreach (AttackType attacktype in availableAttacks)
        {
            string attackTypeName = attacktype.ToString();
            IAction matchingAction = listOfActions.FirstOrDefault(action => action.Name == attackTypeName);
            if (matchingAction != null)
            {
                listOfavailableActions.Add(matchingAction);
            }
        }
        List<IAction> strongestActionList = (from o in listOfavailableActions
                                         orderby o.HPInflicted descending
                                         select o).ToList();
        IAction strongestAction = strongestActionList[0];
        Console.WriteLine($"{monster.Name} is attacking {randomTarget.Name} with a {strongestAction.Name}!");
        Thread.Sleep(2000);
        randomTarget.HP =strongestAction.Hit(randomTarget);
        
        Console.WriteLine($"{randomTarget.Name}'s HP is now {randomTarget.HP}");
        if (randomTarget.HP < 1)
        { 
            randomTarget.IsDead = true;
            Console.WriteLine($"{randomTarget.Name} has been killed.");
        }
            
    }
}
