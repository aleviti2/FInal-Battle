using System.Collections.Generic;

Party party = new Party();
GameEngine gameEngine = new GameEngine(party);

gameEngine.SetHeroesNumber();
gameEngine.ChooseHeroes();

party.AllCharactersList = party.MergeLists(party.HeroesParty, party.MonstersParty);


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
public enum AttackType { Punch, BoneCrunch, MistyFist}
public enum CharacterType { VinFletcher, Tog}
public enum Category { Hero, Skeleton, TheUncodedOne}