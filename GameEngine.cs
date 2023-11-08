using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
public class GameEngine
{
    public int CharactersNumber { get; private set; }
    public int HeroesNumber { get; private set; }
    public List<ICharacter> TurnList { get; set; }
    public Skeleton skeleton1 { get; set; }
    public Skeleton skeleton2 { get; set; }
    public TheUncodedOne uncodedOne { get; set; }
    public Party Party { get; set; }
    public GameEngine(Party party)
    {
        Party = party;
        TurnList = new List<ICharacter>();

        party.AddCharacter(skeleton1 = new Skeleton(1, 0, "Gomer"));
        party.AddCharacter(skeleton2 = new Skeleton(1, 0, "Nefasto"));
        party.AddCharacter(uncodedOne = new TheUncodedOne(1, 0));


    }
    public int SetHeroesNumber()
    {
        Console.WriteLine("Welcome to the dungeon valiant heroes. Enter the number of players");
        HeroesNumber = Convert.ToInt32(Console.ReadLine());
        return HeroesNumber;
    }
    public void ChooseHeroes()
    {
        Console.WriteLine($"You entered: {HeroesNumber}"); // Debug output

        if (HeroesNumber == 2)
        {
            for (int i = 0; i < HeroesNumber; i++)
            {
                Hero hero;
                if (i == 0)
                {
                    hero = new Hero(1, 0, CharacterType.VinFletcher);
                }
                else
                {
                    hero = new Hero(1, 0, CharacterType.Tog);
                }
                hero.GiveName();
                Party.AddCharacter(hero);
            }
        }
        else if (HeroesNumber == 1)
        {
            Hero hero = new Hero(1, 0, CharacterType.VinFletcher);
            hero.GiveName();
            Party.AddCharacter(hero);
        }
        else
        {
            Console.WriteLine("The number of players is not valid. Enter '1' or '2'");
            this.ChooseHeroes();
        }

    }
    public static void Shuffle<T>(List<T> list) //Fisher-Yates shuffle
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public List<ICharacter> CreateTurnList()
    {
        //TurnList.Clear();
        List<ICharacter> availableHeroes = new List<ICharacter>(Party.HeroesParty);
        Shuffle(availableHeroes);
        List<ICharacter> availableMonsters = new List<ICharacter>(Party.MonstersParty);
        Shuffle(availableMonsters);
        TurnList = availableHeroes.Concat(availableMonsters).ToList();
        Console.WriteLine("This is the Turn List:");
        foreach (ICharacter character in TurnList)
        {
            Console.WriteLine(character.Name, character.CharacterCategory);
        }
        return TurnList;
    }
    public void TurnsManager()
    {

        Actions actions = new Actions();
        while (Party.HeroesParty.Any(hero => !hero.IsDead) && Party.MonstersParty.Any(monster => !monster.IsDead)) // LINQ query expression
        {
            //if (Party.HeroesParty.All(hero => hero.IsDead) || Party.MonstersParty.All(monster => monster.IsDead))
            //{
            //    break;
            //}
            foreach (ICharacter character in TurnList.Where(ch => !ch.IsDead))
            {
                //if (character.IsDead)
                //    continue;
                Console.WriteLine($"It's {character.Name}'s turn. Your health points are {character.HP}. Do you want to 'do nothing' or 'attack'?");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "do nothing":
                        Console.WriteLine("You've decided to wait.");
                        continue;
                    case "attack":

                        if (character.CharacterCategory == Category.Hero)
                            AttackTarget(character, Party.MonstersParty, actions);
                        else AttackTarget(character, Party.HeroesParty, actions);

                        break;

                }
            }
        }
            
            if (Party.HeroesParty.Any(hero => !hero.IsDead) && Party.MonstersParty.All(monster => monster.IsDead))
            {
                if (Party.HeroesParty.Count > 1)
                    Console.WriteLine($"Congratulations! The {(Party.HeroesParty[0]).CharacterCategory}s prevailed.The winners are {(Party.HeroesParty[0]).Name} and {(Party.HeroesParty[1])}.");
                else if (Party.HeroesParty.Count == 1 && Party.MonstersParty.All(monster => monster.IsDead))
                    Console.WriteLine($"Congratulations! The {(Party.HeroesParty[0]).CharacterCategory}s prevailed.The winner is {(Party.HeroesParty[0]).Name}");
            }
            else if (Party.MonstersParty.Any(monster => !monster.IsDead) && Party.HeroesParty.All(hero => hero.IsDead))
                Console.WriteLine("The Monsters have prevailed!");
        
    }
    public void AttackTarget(ICharacter attacker, List<ICharacter> targets, Actions action)
    {

        Console.WriteLine("Who would you like to attack?");

        Console.WriteLine("You can attack: ");
        foreach (ICharacter target in targets.Where(target => !target.IsDead))
        {
            Console.WriteLine($"A {target.CharacterCategory} called {target.Name}");
        }
        Console.WriteLine("Type the Name of the character you want to attack, then press Enter");
        string inputCharacterAttacked = Console.ReadLine();
        ICharacter targetCharacter = targets.FirstOrDefault(target => target.Name == inputCharacterAttacked); //LINQ method that returns the first element in a sequence that satisfies a specified condition.FirstOrDefault iterates through the elements in the collection.
                                                                                                              //if (targetMonster == null) continue;
        Console.WriteLine($"{targetCharacter.Name}");
        Console.WriteLine($"What kind of Attack would you like to perform? You have:");                                                                                                                                                  //For each element, it applies the condition specified in the lambda expression.
        foreach (AttackType attack in attacker.AttackT)
        {
            Console.WriteLine($"{attack}");
        }
        string inputAttack = Console.ReadLine();
        AttackType attackSelected = attacker.AttackT.FirstOrDefault(attack => attack.ToString() == inputAttack);
        //if (attackSelected == null) continue;
        switch (attackSelected)
        {
            case AttackType.Punch:
                action.Punch(targetCharacter);
                break;
            case AttackType.BoneCrunch:
                action.BoneCrunch(targetCharacter);
                break;
            case AttackType.MistyFist:
                action.MistyFist(targetCharacter);
                break;
            default:
                Console.WriteLine("Invalid attack type");
                break;
        }
        Console.WriteLine($"Your opponent's HP is now {targetCharacter.HP}");
        targetCharacter.IsDead = AreYouDead(targetCharacter, TurnList);
        //Console.WriteLine(targetCharacter.IsDead);

    }

    public bool AreYouDead(ICharacter cAttacked, List<ICharacter> turnList)
    {
        if (cAttacked.HP <= 0)
        {
            cAttacked.IsDead = true;
            Console.WriteLine($"Your opponent {cAttacked.Name} has been killed.");
            ICharacter turnListCharacter = TurnList.FirstOrDefault(character => character.Name == cAttacked.Name);
            if (turnListCharacter != null)
            {
                turnListCharacter.IsDead = true;
            }
            foreach (ICharacter ch in Party.HeroesParty)
            {
                Console.WriteLine($"{ch.Name} is dead? {ch.IsDead}");
            }
            foreach (ICharacter ch in Party.MonstersParty)
            {
                Console.WriteLine($"{ch.Name} is dead? {ch.IsDead}");
            }
            foreach (ICharacter ch in TurnList)
            {
                Console.WriteLine($"{ch.Name} in TurnList is dead? {ch.IsDead}");
            }
            return true;

            //if (cAttacked.CharacterCategory == Category.Hero)
            //    Party.HeroesParty.Remove(cAttacked);
            //else Party.MonstersParty.Remove(cAttacked);
            
        }
        return false;
    }   
}
