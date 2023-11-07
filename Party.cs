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
    }
    public List<ICharacter> MergeLists(List<ICharacter> heroesParty, List<ICharacter> monstersParty)
    {
        AllCharactersList = heroesParty.Concat(monstersParty).ToList();
        return AllCharactersList;
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
