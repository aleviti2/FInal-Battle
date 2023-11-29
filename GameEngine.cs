﻿using System.Linq;
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
    public ThunderBlast ThunderBlast { get; set; }
    public Annihilator Annihilator { get; set; }
    public bool IsAIActive { get; set; }
    public AttackModifier AttackModifierProperty { get; set; }
    public event Action? BattleEnded;


    public GameEngine(Party party, List<ICharacter> monsters) //First battle
    {
        Party = party;
        TurnList = new List<ICharacter>();
        AttackModifierProperty = new AttackModifier(AttackModifierEnum.NoShield,0,0);
        Party.AttackModifierForAI=AttackModifierProperty;
        Punch = new Punch();
        BoneCrunch = new BoneCrunch();
        Claw = new Claw();
        MistyFist = new MistyFist();
        HealthPotion = new HealthPotion(10);
        ThunderBlast = new ThunderBlast(4);
        Annihilator = new Annihilator(4,8);
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
        ThunderBlast= previousBattle.ThunderBlast;
        Annihilator= previousBattle.Annihilator;
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
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("");
        Console.WriteLine("====================================================");
        Console.WriteLine($"WELCOME TO BATTLE N.{BattleSeries.CurrentBattleNumber}. The Heroes have gained a {AttackModifierProperty.Name} that will protect them. Take good care of it!");
        Console.ResetColor();
    }
    public void InizializeFinalBattle(GameEngine secondGame)
    {
        this.BattleSeries = secondGame.BattleSeries;
        Party.AttackModifierForAI = BattleSeries.AttackModifier;
        BattleEnded += BattleSeries.OnBattleManager;
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("");
        Console.WriteLine("====================================================");
        Console.WriteLine($"WELCOME TO BATTLE N.{BattleSeries.CurrentBattleNumber}. The Heroes have gained a {AttackModifierProperty.Name} that will protect them. Take good care of it!");
        Console.ResetColor();
    }

    public int SetHeroesNumber()
    {
        Console.WriteLine("Welcome to the dungeon valiant heroes. You will have to fight against skeletons, werewolves and the mighty Mephisto, the 'Uncoded One', master of the dungeon. Enter the number of players (you can control 1 or 2 heroes. Input is case insensitive.)");
        do
        {
            int nPlayersOutput;
            
            if (int.TryParse(Console.ReadLine(), out nPlayersOutput) && (nPlayersOutput == 1 || nPlayersOutput == 2))
            { HeroesNumber = nPlayersOutput;
                break;
            }
                
            else Console.WriteLine("Enter 1 or 2");
        } while (true);
        //HeroesNumber = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("If you're playing solo, you may opt to have the game control the monsters. Do you want the monsters to be controlled by an AI?");
        bool isValidInput = false;
        do
        {
            string input = Console.ReadLine();
            if (input != null && (input.ToLower() == "yes" || input.ToLower() == "no"))
            {
                IsAIActive = input.ToLower() == "yes";
                isValidInput = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
            }
        } while (!isValidInput);  
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
                    hero = new Hero(70, 70, 3, "Hero1", CharacterType.VinFletcher);
                }
                else
                {
                    hero = new Hero(70, 70, 3, "Hero2", CharacterType.Tog);
                }
                hero.GiveName();
                Party.AddCharacter(hero);
            }
        }
        else if (HeroesNumber == 1)
        {
            Hero hero = new Hero(140, 140, 5, "Hero1", CharacterType.VinFletcher);
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
        AttackModifierProperty.BuyActions(BattleSeries, AliveHeroes, this);

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
            if (Party.HeroesParty.All(hero => hero.IsDead) && Party.MonstersParty.All(monster => monster.IsDead))
                break;
            
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
                        GameStatus(this.BattleSeries, this.Party, this.AttackModifierProperty);
                        potionTaken = true;
                        bool isValidInput = false;
                        do
                        {
                            Console.WriteLine($"It's {character.Name}'s turn. Their health points are {character.HP}. Do you want to 'do nothing', 'attack' or 'drink potion?'");
                            string input = Console.ReadLine();
                            switch (input.ToLower())
                            {
                                case "do nothing":
                                    Console.WriteLine("You've decided to wait.");
                                    isValidInput = true;
                                    break;
                                //continue;
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
                                    isValidInput = true;
                                    break;
                                //continue;
                                case "drink potion":
                                    {
                                        int hpNow = character.HP;
                                        character.HP = HealthPotion.Hit(character, character); //POTENTIAL ISSUE
                                        if (hpNow == character.HP)
                                            potionTaken = false;
                                        Console.WriteLine($"{character.Name}'s new HP is {character.HP}");
                                    }
                                    isValidInput = true;
                                    break;
                                    //continue;
                            }
                        } while (!isValidInput);
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"***CONGRATULATIONS!*** THE HEROES PREVAILED! The winners are {survivingHeroes[0].Name} and {survivingHeroes[1].Name}.");
                Console.ResetColor();
            }
            else if (Party.HeroesParty.Count(hero => !hero.IsDead) == 1 && Party.MonstersParty.All(monster => monster.IsDead))
            {
                var survivingHeroes = Party.HeroesParty.Where(hero => !hero.IsDead).ToList();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"***CONGRATULATIONS!*** THE HEROES PREVAILED! The winner is {survivingHeroes[0].Name}.");
                Console.ResetColor();
            }
        }
        else if (Party.MonstersParty.Any(monster => !monster.IsDead) && Party.HeroesParty.All(hero => hero.IsDead))
        {  
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("THE MONSTERS HAVE PREVAILED!");
            Console.ResetColor();
        }
            
        else if (Party.HeroesParty.All(hero => hero.IsDead) && Party.MonstersParty.All(monster => monster.IsDead))
        {  
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("THE HEROES KILLED ALL THE MONSTERS BUT DIED IN BATTLE.");
            Console.ResetColor();
        }
            
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
            Console.WriteLine("+++ GAME OVER +++");
        }

    }
    public void AttackTarget(ICharacter attacker, List<ICharacter> targets)
    {

        Console.WriteLine("Who would you like to attack?");

        Console.WriteLine("You can attack: ");
        foreach (ICharacter target in targets.Where(target => !target.IsDead))
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"A {target.CharacterCategory} called {target.Name}");
            Console.ResetColor();
        }
        Console.WriteLine("Type the Name of the character you want to attack, then press Enter");
        string inputCharacterAttacked;
        ICharacter targetCharacter;
        bool isValidInput = false;
        do
        {
            inputCharacterAttacked = Console.ReadLine();
            inputCharacterAttacked.ToLower();
            targetCharacter = targets.FirstOrDefault(t => string.Equals(t.Name, inputCharacterAttacked, StringComparison.OrdinalIgnoreCase)); //Equals returns true/false. FirstOrDefault evaluates lambda expression, if true it returns an ICharacter.

            //targetCharacter = targets.FirstOrDefault(target => target.Name == inputCharacterAttacked);
            if (targetCharacter !=null && !targetCharacter.IsDead)
            { 
                targetCharacter.HitsTakenPerBattle++;
                isValidInput = true;
            }
            else
            {
                Console.WriteLine("Select a valid character.");
            }
        } while (!isValidInput);
        
         //LINQ method that returns the first element in a sequence that satisfies a specified condition.FirstOrDefault iterates through the elements in the collection.
                                                                                                            //if (targetMonster == null) continue;
        //Console.WriteLine($"{targetCharacter.Name}");
        Console.WriteLine($"What kind of Attack would you like to perform? You have:");                                                                                                                                                  //For each element, it applies the condition specified in the lambda expression.
        foreach (AttackType attack in attacker.AttackT)
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine($"{attack}");
            Console.ResetColor();
        }
        bool aSelected = false;
        do
        {
            string inputAttack = Console.ReadLine();
            AttackType attackSelected;
            
            
            if (Enum.TryParse(inputAttack, true, out attackSelected) && attacker.AttackT.Contains(attackSelected, EqualityComparer<AttackType>.Default)) // bool TryParse(string s, bool ignoreCase, out TEnum result);
            {
                aSelected = true;
                //AttackType attackSelected = attacker.AttackT.FirstOrDefault(attack => attack.ToString() == inputAttack);
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
                    case AttackType.ThunderBlast:
                        targetCharacter.HP = ThunderBlast.Hit(targetCharacter, attacker);
                        break;
                    case AttackType.Annihilator:
                        targetCharacter.HP = Annihilator.Hit(targetCharacter, attacker);
                        break;
                    default:
                        Console.WriteLine("Invalid attack type");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Enter a valid attack.");
            }
        } while (!aSelected);
        AttackModifierProperty.CheckForAttackModifier(attacker, targetCharacter);
        //if (targetCharacter.AttackModifier != null && targetCharacter.HitsTakenPerBattle <= AttackModifierProperty.HitsBeforeBreaking)
        //{
        //    AttackModifierProperty.AttackMod(attacker, targetCharacter);
        //}
        //if (targetCharacter.AttackModifier != null && (targetCharacter.HitsTakenPerBattle == AttackModifierProperty.HitsBeforeBreaking+1))
        //{
        //    Console.WriteLine($"{targetCharacter.Name}'s shield is broken.");
        //    Console.WriteLine($"{targetCharacter.Name}'s HP is now {targetCharacter.HP}");
        //}
        //else
        //{
 
        //    Console.WriteLine($"{targetCharacter.Name}'s HP is now {targetCharacter.HP}");
        //}
        
        
        targetCharacter.IsDead = AreYouDead(targetCharacter, TurnList);
        attacker.IsDead = AreYouDead(attacker, TurnList);

    }

    public bool AreYouDead(ICharacter cAttacked, List<ICharacter> turnList)
    {
        if (cAttacked.HP <= 0)
        {
            cAttacked.IsDead = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{cAttacked.Name} has been killed.");
            Console.ResetColor();
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

    public void GameStatus ( BattleSeries battleSeries, Party party, AttackModifier attackModifier)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine($"==================== BATTLE N {battleSeries.CurrentBattleNumber} ==================== ");
        Console.WriteLine();
        foreach (ICharacter hero in Party.HeroesParty )
        {
            if (!hero.IsDead)
            {     //aliveHeroes.Add(hero);
                Console.WriteLine($"{hero.Name}. HP: {hero.HP}/{hero.MaxHP}. Health Potions Available: {hero.PotionsAvailable}. ");
                if (hero.AttackModifier != null && hero.AttackModifier.Name != AttackModifierEnum.NoShield && hero.HitsTakenPerBattle <= attackModifier.HitsBeforeBreaking)
                {
                    int currentHit = attackModifier.HitsBeforeBreaking - hero.HitsTakenPerBattle;
                    Console.WriteLine($"Shield equipped: {hero.AttackModifier.Name}. The shield will protect {hero.Name} for another {currentHit}/{attackModifier.HitsBeforeBreaking} attacks.");
                }
            }
        }
        foreach (ICharacter monster in Party.MonstersParty)
        {
            if (!monster.IsDead)
            {     //aliveHeroes.Add(hero);
                Console.WriteLine($"{monster.Name}. HP: {monster.HP}/{monster.MaxHP}. Health Potions Available: {monster.PotionsAvailable}. ");
            }
        }
        Console.WriteLine();
        Console.WriteLine("====================================================");
        Console.ResetColor();
    }
}

