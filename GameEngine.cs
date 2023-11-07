using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

public class GameEngine
{
    public int CharactersNumber { get; private set; }
    public List<ICharacter> TurnList { get; set; }
    public Skeleton skeleton1 { get; set; }
    public Skeleton skeleton2 { get; set; }
    public TheUncodedOne uncodedOne { get; set; }
    public Party Party { get; set; }
    public GameEngine(Party party, int charactersNumber) 
    {
        Party = party;
        CharactersNumber = charactersNumber;
        TurnList = new List<ICharacter>();

        party.AddCharacter(skeleton1 = new Skeleton(5, 0));
        party.AddCharacter(skeleton2 = new Skeleton(5, 0));
        party.AddCharacter(uncodedOne = new TheUncodedOne(10, 10));

         
    }
    public void ChooseHeroes()
    {
        Console.WriteLine("How many heroes are there? Enter '1' or '2':");
        int playersN = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine($"You entered: {playersN}"); // Debug output

        if (playersN == 2)
        {
            for (int i = 0; i < playersN; i++)
            {
                Hero hero;
                if (i == 0)
                {
                    hero = new Hero(5, 0, CharacterType.VinFletcher);
                }
                else
                {
                    hero = new Hero(4, 0, CharacterType.Tog);
                }
                hero.GiveName();
                Party.AddCharacter(hero);
            }
        }
        else if (playersN == 1)
        {
            Hero hero = new Hero(5, 0, CharacterType.VinFletcher);
            Console.WriteLine("Choose your battle name, valiant warrior, then press enter.");
            hero.GiveName();
            Party.AddCharacter(hero);
        }
        else
        {
            Console.WriteLine("The number of players is not valid. Enter '1' or '2'");
            this.ChooseHeroes();
        }
        
    }
    public List<ICharacter> CreateTurnList()
    {
        TurnList.Clear();
        Random random = new Random();
        List<ICharacter> availableHeroes = new List<ICharacter>(Party.HeroesParty);
        List<ICharacter> availableMonsters = new List<ICharacter>(Party.MonstersParty);

        int totalCHaracters = CharactersNumber;
        
            while (totalCHaracters >0)
            {
                if (availableHeroes.Count > 0)
                {
                    int randomIndexH = random.Next(0, availableHeroes.Count - 1);
                    ICharacter randomHero = availableHeroes[randomIndexH];
                    if (!TurnList.Contains(randomHero))
                    {
                        TurnList.Add(randomHero);
                        availableHeroes.Remove(randomHero);
                        totalCHaracters--;
                    }
                }
                if (availableMonsters.Count > 0)
                {
                    int randomIndexM = random.Next(0, availableMonsters.Count - 1);
                    ICharacter randomMonster = availableMonsters[randomIndexM];
                    if (!TurnList.Contains(randomMonster))
                    {
                        TurnList.Add(randomMonster);
                        availableMonsters.Remove(randomMonster);
                        totalCHaracters--;
                    }
                }
            }
        
        Console.WriteLine("This is the Turn List:");
        foreach (ICharacter character in TurnList)
        {        
            Console.WriteLine(character.CharacterCategory);       
        }
        return TurnList;
    }
    public void TurnsManager()
    {
        Actions actions = new Actions();
        foreach (ICharacter character in TurnList) 
        {
                Console.WriteLine($"It's {character.Name}'s turn. Your health points are {character.HP}. Do you want to 'do nothing' or 'attack'?");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "do nothing":
                        Console.WriteLine("You've decided to wait.");
                        continue;
                    case "attack":
                        if (character.CharacterCategory == Category.Hero)
                        {
                            Console.WriteLine("Who would you like to attack?");
                            
                            Console.WriteLine("You can attack: ");
                            foreach (ICharacter monster in Party.MonstersParty)
                            {
                                Console.WriteLine($"A {monster.CharacterCategory} called {monster.Name}");                               
                            }
                            Console.WriteLine("Type the Name of the monster you want to attack, then press Enter");
                            string inputCharacterAttacked = Console.ReadLine();
                            ICharacter targetMonster = Party.MonstersParty.FirstOrDefault(monster => monster.Name == inputCharacterAttacked.ToLower()); //LINQ method that returns the first element in a sequence that satisfies a specified condition.FirstOrDefault iterates through the elements in the collection.
                            //if (targetMonster == null) continue;
                            Console.WriteLine($"What kind of Attack would you like to perform? You have:");                                                                                                                                                  //For each element, it applies the condition specified in the lambda expression.
                            foreach (AttackType attacks in character.AttackT)
                            {
                                Console.WriteLine(character.AttackT);
                            }
                            string inputAttack = Console.ReadLine();
                            AttackType attackSelected = character.AttackT.FirstOrDefault(attack => attack.ToString() == inputAttack);
                            //if (attackSelected == null) continue;
                            switch (attackSelected)
                            {
                                case AttackType.Punch:
                                    actions.Punch(targetMonster);
                                    break;
                                case AttackType.BoneCrunch:
                                actions.BoneCrunch(targetMonster);
                                    break;
                                default:
                                    Console.WriteLine("Invalid attack type");
                                    break;
                            }
                            Console.WriteLine($"Your opponent's HP is now {targetMonster.HP}");
                            AreYouDead(targetMonster, TurnList);
                        }
                        break;
                }
        }
    }
    public bool AreYouDead(ICharacter cAttacked, List<ICharacter> turnList)
    {
        if (cAttacked.HP <=0)
        { 
            cAttacked.IsDead = true;
            TurnList.Remove(cAttacked);
            Console.WriteLine($"Your opponent {cAttacked.Name} has been killed.");
            return cAttacked.IsDead;
        }
        return false;
    }   
}
