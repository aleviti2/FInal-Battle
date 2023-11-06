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
    public Party party { get; set; }
    public GameEngine(int charactersNumber) 
    {
        party = new Party();
        CharactersNumber = charactersNumber;
        TurnList = new List<ICharacter>();

        party.AddCharacter(skeleton1 = new Skeleton(5, 0, "Growly"));
        party.AddCharacter(skeleton2 = new Skeleton(5, 0, "Twiggy"));
        party.AddCharacter(uncodedOne = new TheUncodedOne(10, 10));

        
    }
    public void ChooseHeroes( )
    {
        Console.WriteLine("How many heroes are there? Enter '1' or '2':");
        int playersN = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine($"You entered: {playersN}"); // Debug output
        if (playersN == 2)
        {
            party.AddCharacter(new Hero(5, 0, CharacterType.VinFletcher, "Mario"));
            party.AddCharacter(new Hero(4, 0, CharacterType.Tog, "Luigi"));
        }
        else if (playersN == 1)
        {
            party.AddCharacter(new Hero(5, 0, CharacterType.VinFletcher, "Mario"));
        }
        else
        {
            Console.WriteLine("The number of players is not valid. Enter '1' or '2'");
            this.ChooseHeroes();
        }
        Console.WriteLine("Characters in party:");
        foreach (var character in party.MonstersParty)
        {
            Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
        }
        foreach (var character in party.HeroesParty)
        {
            Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
        }
    }
    public List<ICharacter> CreateTurnList()
    {
        while (TurnList.Count <= CharactersNumber)
        {
            Random random = new Random();
            while (true)
            {
                int randomIndexH = random.Next(0, party.HeroesParty.Count);
                ICharacter randomHero = party.HeroesParty[randomIndexH];
                if (!TurnList.Contains(randomHero))
                {
                    TurnList.Add(randomHero);
                    break;
                }
            }

            while (true)
            {
                int randomIndexM = random.Next(0, party.MonstersParty.Count);
                ICharacter randomMonster = party.MonstersParty[randomIndexM];
                if (!TurnList.Contains(randomMonster))
                {
                    TurnList.Add(randomMonster);
                    break;
                }
            }
        }
        foreach (var character in TurnList)
        {        
            Console.WriteLine(character.Name);       
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
                            foreach (ICharacter monster in party.MonstersParty)
                            {
                                Console.WriteLine($"A {monster.CharacterCategory} called {monster.Name}");                               
                            }
                            Console.WriteLine("Type the Name of the monster you want to attack, then press Enter");
                            string inputCharacterAttacked = Console.ReadLine();
                            ICharacter targetMonster = party.MonstersParty.FirstOrDefault(monster => monster.Name == inputCharacterAttacked.ToLower()); //LINQ method that returns the first element in a sequence that satisfies a specified condition.FirstOrDefault iterates through the elements in the collection.
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
