public class Actions
{
    public void Punch (ICharacter characterPunched)
    {     
        characterPunched.HP -= 1;
    }
    public void BoneCrunch (ICharacter characterCrunched)
    {
        Random random = new Random();
        int pointsTaken = random.Next(0, 1);
        characterCrunched.HP -= pointsTaken;
        Console.WriteLine($"{characterCrunched.Name} has lost {pointsTaken} health points. Points remaining {characterCrunched.HP}");
    }
}
