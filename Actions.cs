public class Actions
{
    public void Punch (ICharacter characterPunched)
    {     
        characterPunched.HP -= 1;
        Console.WriteLine($"{characterPunched.Name} has lost {1} health point. Points remaining {characterPunched.HP}");
    }
    public void BoneCrunch (ICharacter characterCrunched)
    {
        Random random = new Random();
        int pointsTaken = random.Next(0, 2);
        characterCrunched.HP -= pointsTaken;
        Console.WriteLine($"{characterCrunched.Name} has lost {pointsTaken} health point. Points remaining {characterCrunched.HP}");
    }

    public void MistyFist(ICharacter characterMisty)
    {
        Random random = new Random();
        int pointsTaken = random.Next(0, 3);
        characterMisty.HP -= pointsTaken;
        Console.WriteLine($"{characterMisty.Name} has lost {pointsTaken} health point. Points remaining {characterMisty.HP}");
    }
}
