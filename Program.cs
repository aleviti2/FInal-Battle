using System.Collections.Generic;
GameEngine gameEngine = new GameEngine(5);


Party party = new Party();
gameEngine.ChooseHeroes();

party.AllCharactersList = party.MergeLists(party.HeroesParty, party.MonstersParty);
foreach (ICharacter character in party.AllCharactersList)
{
    Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
}

//Console.WriteLine($"First character in CharactersList: Name HP - {party.AllCharactersList[0].HP}");

//gameEngine.CreateTurnList();
//gameEngine.TurnsManager();
public enum AttackType { Punch, BoneCrunch, MistyFist}
public enum CharacterType { VinFletcher, Tog}
public enum Category { Hero, Skeleton, TheUncodedOne}