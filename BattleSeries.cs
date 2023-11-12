using System.Security.Cryptography.X509Certificates;

public class BattleSeries
{ 
    public GameEngine GameEngineProperty { get; set; }
    public GameEngine NewBattle { get; set; }
    public int CurrentBattleNumber { get; set; } = 1;
    (string, int, int) ScoresByPlayer { get; set; }
    public TheUncodedOne UncodedOne { get; set; }
    public BattleSeries(GameEngine engine)
    {
        GameEngineProperty = engine;
        //engine.BattleEnded += OnBattleManager;
    }

    public void OnBattleManager() => ResetToNewBattle();
    
    public void ResetToNewBattle()
    {
        if (CurrentBattleNumber == 2)
        {
            NewBattle = new GameEngine(GameEngineProperty);
        }
        if (CurrentBattleNumber == 3) 
        {
            UncodedOne = new TheUncodedOne(1, 0);
            NewBattle= new GameEngine(GameEngineProperty, UncodedOne);
        }
    }


   

}
