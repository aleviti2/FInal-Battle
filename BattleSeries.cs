
public class BattleSeries
{ 
    public GameEngine GameEngineProperty { get; set; }
    public GameEngine SecondBattle { get; set; }
    public GameEngine ThirdBattle { get; set; }
    //public GameEngine NewBattle { get; set; }
    public int CurrentBattleNumber { get; set; } = 1;
    (string, int, int) ScoresByPlayer { get; set; }
    public TheUncodedOne UncodedOne { get; set; }
    public BattleSeries(GameEngine engine)
    {
        GameEngineProperty = engine;
        //engine.BattleEnded += OnBattleManager;
    }
    public void BattleCounter()
    {

    }
    public void OnBattleManager() => ResetToNewBattle();
    
    public void ResetToNewBattle()
    {
        if (CurrentBattleNumber == 2)
        { 
            SecondBattle = new GameEngine(GameEngineProperty ,new List<ICharacter> { new Werewolf(1,0,"Romulus"), new Werewolf(1, 0, "Remus")});          
            
            SecondBattle.InizializeSecondBattle(GameEngineProperty);
            SecondBattle.IsAIActive = GameEngineProperty.IsAIActive;
            SecondBattle.CreateTurnList();
            SecondBattle.TurnsManager();
            SecondBattle.InvokeOrEnd();
            return;
        }
        else if (CurrentBattleNumber == 3) 
        {
            ThirdBattle = new GameEngine(SecondBattle, new List<ICharacter> { new TheUncodedOne(1,0)});

            ThirdBattle.InizializeFinalBattle(SecondBattle);
            ThirdBattle.IsAIActive = SecondBattle.IsAIActive;
            ThirdBattle.CreateTurnList();
            ThirdBattle.TurnsManager();
            ThirdBattle.InvokeOrEnd();
            return;
        }
    }
}
