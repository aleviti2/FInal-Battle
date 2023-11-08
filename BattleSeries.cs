public class BattleSeries
{ 
    public GameEngine GameEngineProperty { get; set; }
    public int BattleSeriesNumber { get; set; }

    public BattleSeries(GameEngine engine)
    { 
        GameEngineProperty = engine; 
    }

}
