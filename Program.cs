namespace CarGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (Tests.AnimationTest game = new Tests.AnimationTest())
            using (Tests.CarBuildScreen game = new Tests.CarBuildScreen())
            //using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}