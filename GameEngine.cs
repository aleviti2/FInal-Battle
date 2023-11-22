using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
public class GameEngine
{
    public int HeroesNumber { get; private set; }
    public List<ICharacter> TurnList { get; set; }
    public Party Party { get; set; }
    public BattleSeries BattleSeries { get; set; }
    public List<ICharacter> AliveHeroes { get; set; }
    public Punch Punch { get; set; }
    public BoneCrunch BoneCrunch { get; set; }
    public Claw Claw { get; set; }
    public MistyFist MistyFist { get; set; }
    public HealthPotion HealthPotion { get; set; }
    public bool IsAIActive { get; set; }
    public AttackModifier AttackModifierProperty { get; set; }
    public event Action? BattleEnded;


    public GameEngine(Party party, List<ICharacter> monsters) //First battle
    {
        Party = party;
        TurnList = new List<ICharacter>();

        Punch = new Punch();
        BoneCrunch = new BoneCrunch();
        Claw = new Claw();
        MistyFist = new MistyFist();
        HealthPotion = new HealthPotion(1);

        foreach (ICharacter character in monsters)
        {
            Party.AddCharacter(character);
        }
    }

    public GameEngine(GameEngine previousBattle, List<ICharacter> monsters) //Next Battles
    {

        Party = previousBattle.Party;
        TurnList = new List<ICharacter>();
        //BattleSeries.PartyForAttackModifier = Party;
        Party.GameEngineForAddShield = previousBattle;
        Punch = previousBattle.Punch;
        BoneCrunch = previousBattle.BoneCrunch;
        Claw = previousBattle.Claw;
        MistyFist = previousBattle.MistyFist;
        HealthPotion= previousBattle.HealthPotion;
        List<ICharacter> survivingCharacters = previousBattle.Party.HeroesParty.Where(hero => !hero.IsDead).ToList();
        AliveHeroes = survivingCharacters;
        Party.MonstersParty = new List<ICharacter>();

        foreach (ICharacter character in monsters)
        {
            Party.AddCharacter(character);
        }

    }
    public void InitializeBattleSeries()
    {
        BattleSeries = new BattleSeries(this);
        BattleEnded += BattleSeries.OnBattleManager;
    }
    public void InizializeSecondBattle(GameEngine firstGame)
    {
        this.BattleSeries = firstGame.BattleSeries;
        Party.AttackModifierForAI = BattleSeries.AttackModifier;     
        BattleEnded += BattleSeries.OnBattleManager;
        Console.WriteLine($"Welcome to battle N.{BattleSeries.CurrentBattleNumber}. The Heroes have gained a {AttackModifierProperty.Name} that will protect them. Take good care of it!");
    }
    public void InizializeFinalBattle(GameEngine secondGame)
    {
        this.BattleSeries = secondGame.BattleSeries;
        Party.AttackModifierForAI = BattleSeries.AttackModifier;
        BattleEnded += BattleSeries.OnBattleManager;
        Console.WriteLine($"Welcome to battle N.{BattleSeries.CurrentBattleNumber}. The Heroes have gained a {AttackModifierProperty.Name} that will protect them. Take good care of it!");
    }

    public int SetHeroesNumber()
    {
        Console.WriteLine("Welcome to the dungeon valiant heroes. Enter the number of players");
        HeroesNumber = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Do you want the monsters to be controlled by an AI?");
        string input = Console.ReadLine();
        if (input != null && input == "Yes")
            IsAIActive = true;
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
                    hero = new Hero(4, 4, 1, "Hero1", CharacterType.VinFletcher);
                }
                else
                {
                    hero = new Hero(4, 4, 1, "Hero2", CharacterType.Tog);
                }
                hero.GiveName();
                Party.AddCharacter(hero);
            }
        }
        else if (HeroesNumber == 1)
        {
            Hero hero = new Hero(4, 4, 1, "Hero1", CharacterType.VinFletcher);
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

        bool exitBothLoops = false;
        while (true)
        {
            //Console.WriteLine($"The turnListCOPY contains {TurnList.Count}");


            //Console.WriteLine($"The HeroesParty list contains {Party.HeroesParty.Count}. The MonsterParty list contains {Party.MonstersParty.Count}");
            foreach (ICharacter character in TurnList.Where(ch => !ch.IsDead))
            {
                if (Party.HeroesParty.All(hero => hero.IsDead) || Party.MonstersParty.All(monster => monster.IsDead))
                {

                    exitBothLoops = true;
                    break;
                }

                if (IsAIActive && Party.MonstersParty.Contains(character))
                    this.Party.AIMonsters(character);
                else
                {
                    bool potionTaken = false;
                    while (!potionTaken)
                    {
                        potionTaken = true;
                        Console.WriteLine($"It's {character.Name}'s turn. Their health points are {character.HP}. Do you want to 'do nothing', 'attack' or 'drink potion?'");
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
                                    AttackTarget(character, Party.MonstersParty);
                                }
                                else
                                {
                                    Console.WriteLine("Attacking a hero");
                                    AttackTarget(character, Party.HeroesParty);
                                }
                                continue;
                            case "drink potion":
                                {
                                    int hpNow = character.HP;
                                    character.HP = HealthPotion.Hit(character,character); //POTENTIAL ISSUE
                                    if (hpNow == character.HP)
                                        potionTaken = false;
                                    Console.WriteLine($"{character.Name}'s new HP is {character.HP}");
                                }
                                continue;
                        }
                    }
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
        foreach (ICharacter hero in Party.HeroesParty)
        {
            hero.HitsTakenPerBattle= 0;
        }
    }
    public void InvokeOrEnd()
    {
        //Console.WriteLine($"WE JUST FINISHED BATTLE n {BattleSeries.CurrentBattleNumber}");
        BattleSeries.CurrentBattleNumber++;
        //Console.WriteLine($"WE are ABOUT TO START BATTLE n {BattleSeries.CurrentBattleNumber}");
        if (BattleSeries.CurrentBattleNumber < 4 && Party.HeroesParty.Any(hero => !hero.IsDead))
        {
            BattleEnded.Invoke();
        }
        else
        {
            Console.WriteLine("The game has concluded.");
        }

    }
    public void AttackTarget(ICharacter attacker, List<ICharacter> targets)
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
        targetCharacter.HitsTakenPerBattle++;                                                                                                    //if (targetMonster == null) continue;
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
                targetCharacter.HP = Punch.Hit(targetCharacter, attacker);
                break;
            case AttackType.BoneCrunch:
                targetCharacter.HP = BoneCrunch.Hit(targetCharacter, attacker);
                break;
            case AttackType.MistyFist:
                targetCharacter.HP = MistyFist.Hit(targetCharacter, attacker);
                break;
            case AttackType.Claw:
                targetCharacter.HP = Claw.Hit(targetCharacter, attacker);
                break;
            default:
                Console.WriteLine("Invalid attack type");
                break;
        }
        if (targetCharacter.AttackModifier != null && targetCharacter.HitsTakenPerBattle <= AttackModifierProperty.HitsBeforeBreaking)
        {
            AttackModifierProperty.AttackMod(attacker, targetCharacter);
        }
        if (targetCharacter.AttackModifier != null && (targetCharacter.HitsTakenPerBattle == AttackModifierProperty.HitsBeforeBreaking+1))
        {
            Console.WriteLine($"{targetCharacter.Name}'s shield is broken.");
            Console.WriteLine($"{targetCharacter.Name}'s HP is now {targetCharacter.HP}");
        }
        else
        {
 
            Console.WriteLine($"{targetCharacter.Name}'s HP is now {targetCharacter.HP}");
        }
        
        
        targetCharacter.IsDead = AreYouDead(targetCharacter, TurnList);

    }

    public bool AreYouDead(ICharacter cAttacked, List<ICharacter> turnList)
    {
        if (cAttacked.HP <= 0)
        {
            cAttacked.IsDead = true;
            Console.WriteLine($"Your opponent {cAttacked.Name} has been killed.");

            ICharacter turnListHero = Party.HeroesParty.FirstOrDefault(character => character.Name == cAttacked.Name);

            if (turnListHero != null)
            {
                turnListHero.IsDead = true;
            }
            ICharacter turnListMonster = Party.MonstersParty.FirstOrDefault(character => character.Name == cAttacked.Name);
            if (turnListMonster != null)
            {
                turnListMonster.IsDead = true;
            }

            return true;
        }
        return false;
    }

}