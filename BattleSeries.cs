
public class BattleSeries
{ 
    public GameEngine GameEngineProperty { get; set; }
    public GameEngine SecondBattle { get; set; }
    public GameEngine ThirdBattle { get; set; }
    //public GameEngine NewBattle { get; set; }
    public int CurrentBattleNumber { get; set; } = 1;
    //(string, int, int) ScoresByPlayer { get; set; }
    //public TheUncodedOne UncodedOne { get; set; }
     public Party ForAttackModifier { get; set; }
   
    public AttackModifier AttackModifier { get; set; }
    public BattleSeries(GameEngine engine)
    {
        GameEngineProperty = engine;       
    }

    public void OnBattleManager() => ResetToNewBattle();
    
    public void ResetToNewBattle()
    {
        if (CurrentBattleNumber == 2)
        { 
            SecondBattle = new GameEngine(GameEngineProperty ,new List<ICharacter> { new Werewolf(30,30,1,"Romulus"), new Werewolf(30,30,1,"Remus")});
            AttackModifier silverShield = new AttackModifier(AttackModifierEnum.SilverShield, 4, 2 );
            SecondBattle.AttackModifierProperty = silverShield;
            AttackModifier = silverShield;
            SecondBattle.InizializeSecondBattle(GameEngineProperty);
            ForAttackModifier = SecondBattle.Party;
            ForAttackModifier.AddShield(2);
            SecondBattle.IsAIActive = GameEngineProperty.IsAIActive;
            SecondBattle.CreateTurnList();
            SecondBattle.TurnsManager();
            
            SecondBattle.InvokeOrEnd();
            return;
        }
        else if (CurrentBattleNumber == 3) 
        {
            ThirdBattle = new GameEngine(SecondBattle, new List<ICharacter> { new TheUncodedOne(40,40,2)});
            AttackModifier goldenShield = new AttackModifier(AttackModifierEnum.GoldenShield, 5, 3);
            ThirdBattle.AttackModifierProperty = goldenShield;
            AttackModifier = goldenShield;
            ThirdBattle.InizializeFinalBattle(SecondBattle);
            ForAttackModifier= ThirdBattle.Party;
            ForAttackModifier.AddShield(3);
            ThirdBattle.IsAIActive = SecondBattle.IsAIActive;
            ThirdBattle.CreateTurnList();
            ThirdBattle.TurnsManager();
            ThirdBattle.InvokeOrEnd();
            return;
        }
    }
}
