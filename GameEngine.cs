using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
public class GameEngine
{
    //public int CharactersNumber { get; private set; }
     //public Skeleton skeleton1 { get; set; }
    //public Skeleton skeleton2 { get; set; }
    //public TheUncodedOne uncodedOne { get; set; }
    public int HeroesNumber { get; private set; }
    public List<ICharacter> TurnList { get; set; }
    public Party Party { get; set; }
    public BattleSeries BattleSeries { get; set; }
    public List<ICharacter> AliveHeroes { get; set; }

    public event Action? BattleEnded;

    public GameEngine(Party party) //First battle
    {
        Party = party;
        TurnList = new List<ICharacter>();
        //this.battleSeries = battleSeries;
        
        party.AddCharacter(/*skeleton1 = */new Skeleton(1, 0, "Gomer"));
        party.AddCharacter(/*skeleton2 = */new Skeleton(1, 0, "Nefasto"));
        //party.AddCharacter(uncodedOne = new TheUncodedOne(1, 0));
        BattleSeries = new BattleSeries(this);
        BattleEnded += BattleSeries.OnBattleManager;
    }

    public GameEngine(GameEngine firstGame) //Second Battle
    {
        this.BattleSeries = firstGame.BattleSeries;                                                             // The first BattleSeries object is reused with all its properties and data. 
        Party = firstGame.Party;                                                                                // The first Party object is reused with all its properties and data. 
        TurnList = new List<ICharacter>();                                                                      // I created a new TurnList property as the characters will be different.
        BattleEnded += BattleSeries.OnBattleManager;                                                            // I subscribed to the Event BattleEnded.
        List<ICharacter> survivingCharacters = firstGame.Party.HeroesParty.Where(hero => !hero.IsDead).ToList();// HeroesParty is property of Party property of the firstGame object, which is transferred into the Party property if this(second) instance of Party
        AliveHeroes = survivingCharacters;                                                                      // The new list of surviving heroes is transferred into AliveHeroes, which is populated only when second GameObject constructor is called.
        Party.MonstersParty = new List<ICharacter>();         

        Party.AddCharacter(new Werewolf(1, "Remus"));
        Party.AddCharacter(new Werewolf(1, "Romulus"));

        CreateTurnList();
        TurnsManager();
        
    }

    public GameEngine(GameEngine secondGame, ICharacter theUncodedOne) //Final battle
    {
        this.BattleSeries = secondGame.BattleSeries;
        Party = secondGame.Party;
        TurnList = new List<ICharacter>();
        BattleEnded += BattleSeries.OnBattleManager;
        List<ICharacter> survivingCharacters = secondGame.Party.HeroesParty.Where(hero => !hero.IsDead).ToList(); // HeroesParty is property of Party property of the firstGame object
        AliveHeroes = survivingCharacters;
        Party.MonstersParty = new List<ICharacter>();        //or Party.MonstersParty.Clear(); 

        Party.AddCharacter(new TheUncodedOne(1, 0));
 
        CreateTurnList();
        TurnsManager();
        
    }
  
    public int SetHeroesNumber()
    {
        Console.WriteLine("Welcome to the dungeon valiant heroes. Enter the number of players");
        HeroesNumber = Convert.ToInt32(Console.ReadLine());
        return HeroesNumber;
    }
    public void ChooseHeroes()
    {
        ///*Console.WriteLine($"You entered: {HeroesNumber}"); //*/ Debug output

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
        List<ICharacter> availableHeroes;
        //TurnList.Clear();
        if (AliveHeroes == null)
        {
          availableHeroes = new List<ICharacter>(Party.HeroesParty);
        }
        else { availableHeroes = new List<ICharacter>(AliveHeroes); }
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
        //List<ICharacter> turnListCopy = new List<ICharacter>(TurnList);
        bool exitBothLoops = false;
        while (true)
        {
            //Console.WriteLine($"The turnListCOPY contains {TurnList.Count}");

            
            //Console.WriteLine($"The HeroesParty list contains {Party.HeroesParty.Count}. The MonsterParty list contains {Party.MonstersParty.Count}");
            foreach (ICharacter character in TurnList.Where(ch => !ch.IsDead))
            {
                if (Party.HeroesParty.All(hero => hero.IsDead) || Party.MonstersParty.All(monster => monster.IsDead))
                {
                    //Console.WriteLine($"Exiting both loops");
                    exitBothLoops = true;
                    break;
                }
                Console.WriteLine($"It's {character.Name}'s turn. Their health points are {character.HP}. Do you want to 'do nothing' or 'attack'?");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "do nothing":
                        Console.WriteLine("You've decided to wait.");
                        continue;
                    case "attack":

                        if (character.CharacterCategory == Category.Hero)
                        {
                            Console.WriteLine("Attacking a monster");
                            AttackTarget(character, Party.MonstersParty, actions);
                            
                        }
                        else
                        {
                            Console.WriteLine("Attacking a hero");
                            AttackTarget(character, Party.HeroesParty, actions);
                            
                        }
                        continue;

                }
            }
            if (exitBothLoops)
            {
                break; // Exit both loops
            }
        }

        if (Party.HeroesParty.Any(hero => !hero.IsDead) && Party.MonstersParty.All(monster => monster.IsDead))
        {
            if (Party.HeroesParty.Count(hero => !hero.IsDead) == 2)
            {
                var survivingHeroes = Party.HeroesParty.Where(hero => !hero.IsDead).ToList();
                Console.WriteLine($"Congratulations! The Heroes prevailed. The winners are {survivingHeroes[0].Name} and {survivingHeroes[1].Name}.");
            }
            else if (Party.HeroesParty.Count(hero => !hero.IsDead) == 1 && Party.MonstersParty.All(monster => monster.IsDead))
            {
                var survivingHeroes = Party.HeroesParty.Where(hero => !hero.IsDead).ToList();
                Console.WriteLine($"Congratulations! The Heroes prevailed. The winner is {survivingHeroes[0].Name}.");
            }
        }
        else if (Party.MonstersParty.Any(monster => !monster.IsDead) && Party.HeroesParty.All(hero => hero.IsDead))
            Console.WriteLine("The Monsters have prevailed!");

        BattleSeries.CurrentBattleNumber++;
        if (BattleSeries.CurrentBattleNumber < 4 && Party.HeroesParty.Any(hero => !hero.IsDead))
        { 
            BattleEnded.Invoke();
        }
        else
        {
            Console.WriteLine("The game has concluded.");
        }
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
            case AttackType.Claw:
                action.Claw(targetCharacter);
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
           
                ICharacter turnListHero = Party.HeroesParty.FirstOrDefault(character => character.Name == cAttacked.Name);
            //turnListCharacter.IsDead = true;
            
            if (turnListHero != null)
            {
                turnListHero.IsDead = true;
            }
            ICharacter turnListMonster = Party.MonstersParty.FirstOrDefault(character => character.Name == cAttacked.Name);
            if (turnListMonster != null)
            {
                turnListMonster.IsDead = true;
            }
            //foreach (ICharacter ch in Party.HeroesParty)
            //{
            //    Console.WriteLine($"{ch.Name} in HeroesParty is dead? {ch.IsDead}.");
            //}
            //foreach (ICharacter ch in Party.MonstersParty)
            //{
            //    Console.WriteLine($"{ch.Name} in MonsterParty is dead? {ch.IsDead}.");
            //}
            //foreach (ICharacter ch in TurnList)
            //{
            //    Console.WriteLine($"{ch.Name} in TurnList is dead? {ch.IsDead}.");
            //} 
            return true;
        }
        return false;
    }   
}
