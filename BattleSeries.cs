public class BattleSeries
{ 
    public GameEngine GameEngineProperty { get; set; }
    public int CurrentBattleNumber { get; set; }
    (string, int, int) ScoresByPlayer { get; set; }

    public BattleSeries(GameEngine engine)
    { 
        GameEngineProperty = engine; 
    }



}
