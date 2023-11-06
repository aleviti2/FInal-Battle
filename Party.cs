using System.Collections;

public class Party : IEnumerable<ICharacter>
{
    public List<ICharacter> AllCharactersList { get; set; }
    public List<ICharacter> HeroesParty { get; set; }
    public List<ICharacter> MonstersParty { get; set; }


    public Party(  )
    {
        AllCharactersList = new List<ICharacter>();
        HeroesParty = new List<ICharacter>();
        MonstersParty = new List<ICharacter>();
    }
  
    public void AddCharacter(ICharacter ch)
    {

        if (ch is Hero hero)
        {
            HeroesParty.Add(ch);

        }
        else
        {
            MonstersParty.Add(ch);
        }
        AllCharactersList.Add(ch);
       
    }
    public List<ICharacter> MergeLists(List<ICharacter> heroesParty, List<ICharacter> monstersParty)
    {
        AllCharactersList.AddRange(heroesParty);
        AllCharactersList.AddRange(monstersParty);
        Console.WriteLine("Monsters Party:");
        foreach (ICharacter character in AllCharactersList)
        {
            Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
        }
        return AllCharactersList;


        //Console.WriteLine("MergeLists method called.");
        //if (heroesParty.Any())
        //{
        //    Console.WriteLine("Heroes Party:");
        //    foreach (var character in heroesParty)
        //    {
        //        Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
        //    }
        //}

        //if (monstersParty.Any())
        //{
        //    Console.WriteLine("Monsters Party:");
        //    foreach (var character in monstersParty)
        //    {
        //        Console.WriteLine($"Character Name: {character.Name}, HP: {character.HP}");
        //    }
        //}

        //AllCharactersList = heroesParty.Concat(monstersParty).ToList();


        //return AllCharactersList;
    }

    public IEnumerator<ICharacter> GetEnumerator()
    {
        return AllCharactersList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
