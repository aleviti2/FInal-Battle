using System.Collections.Generic;

Party party = new Party();

GameEngine gameEngine = new GameEngine(party, new List<ICharacter> { new Skeleton(4,4, 1, "Gomer"), new Skeleton(4,4, 1, "Nefasto")});


gameEngine.SetHeroesNumber();
gameEngine.ChooseHeroes();

party.AllCharactersList = party.MergeLists(party.HeroesParty, party.MonstersParty);
gameEngine.InitializeBattleSeries();

//foreach (ICharacter character in party.MonstersParty)
//{
//    Console.WriteLine($"Character Name: {character.CharacterCategory}, HP: {character.HP}");
//}
//foreach (ICharacter character in party.HeroesParty)
//{
//    Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
//}

gameEngine.CreateTurnList();

gameEngine.TurnsManager();
gameEngine.InvokeOrEnd();
public enum AttackType { Punch, BoneCrunch, Claw, MistyFist }
public enum CharacterType { VinFletcher, Tog }
public enum Category { Hero, Skeleton, Werewolf, TheUncodedOne }