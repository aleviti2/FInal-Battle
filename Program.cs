using System.Collections.Generic;

Party party = new Party();
GameEngine gameEngine = new GameEngine(party,4);
Console.WriteLine(gameEngine.CharactersNumber);

gameEngine.ChooseHeroes();

party.AllCharactersList = party.MergeLists(party.HeroesParty, party.MonstersParty);


foreach (ICharacter character in party.MonstersParty)
{
    Console.WriteLine($"Character Name: {character.CharacterCategory}, HP: {character.HP}");
}
foreach (ICharacter character in party.HeroesParty)
{
    Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
}



gameEngine.CreateTurnList();
//Console.WriteLine($"First member of the turnList {gameEngine.TurnList[0]}");
//gameEngine.TurnsManager();
public enum AttackType { Punch, BoneCrunch, MistyFist}
public enum CharacterType { VinFletcher, Tog}
public enum Category { Hero, Skeleton, TheUncodedOne}