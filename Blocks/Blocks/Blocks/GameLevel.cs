namespace Blocks
{
    public class GameLevel
    {
        public Level Level { get; set; }

        public GameLevel(Level level)
        {
            Level = level;
        }

        public double GetFallSpeed()
        {
            return Level switch
            {
                Level.Easy => 1.0,
                Level.Medium => 2.0,
                Level.Hard => 3.0,
                _ => 1.0,
            };
        }

        public int GetBlockSize()
        {
            return Level switch
            {
                Level.Easy => 60,
                Level.Medium => 40,
                Level.Hard => 30,
                _ => 60,
            };
        }

    }


}
