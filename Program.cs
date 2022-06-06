namespace CarGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (Tests.AnimationTest game = new Tests.AnimationTest())
            //using (Tests.CarBuildScreen game = new Tests.CarBuildScreen())
            using (Tests.EngineSound game = new Tests.EngineSound())
            //using (Game1 game = new Game1())
            //using (Tests.CarMovementScreen game = new Tests.CarMovementScreen())
            {
                game.Run();
            }
        }
    }
}