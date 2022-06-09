using Microsoft.Xna.Framework;

namespace CarGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            args = new string[1] { "1" };
            if (args.Length == 0)
            {
                using (Game1 game = new Game1())
                { game.Run(); }
            }
            else
            {
                if (args[0] == "1")
                {
                    using (Tests.MultiDesertBiome game = new Tests.MultiDesertBiome())
                    { game.Run(); }
                }
                else if (args[0] == "2")
                {
                    using (Tests.AnimationTest game = new Tests.AnimationTest())
                    { game.Run(); }
                }
                else if (args[0] == "3")
                {
                    using (Tests.EngineSound game = new Tests.EngineSound())
                    { game.Run(); }
                }
                else if (args[0] == "4")
                {
                    using (Tests.CarMovementScreen game = new Tests.CarMovementScreen())
                    { game.Run(); }
                }
                else if (args[0] == "5")
                {
                    using (Tests.CarBuildScreen game = new Tests.CarBuildScreen())
                    { game.Run(); }
                }
                else
                {
                    using (Game1 game = new Game1())
                    { game.Run(); }
                }
            }

            ////using (Tests.AnimationTest game = new Tests.AnimationTest())
            ////using (Tests.CarBuildScreen game = new Tests.CarBuildScreen())
            //using (Tests.MultiDesertBiome game = new Tests.MultiDesertBiome())
            ////using (Tests.AncientRuinsParallax game = new Tests.AncientRuinsParallax())
            ////using (Tests.EngineSound game = new Tests.EngineSound())
            ////using (Game1 game = new Game1())
            ////using (Tests.CarMovementScreen game = new Tests.CarMovementScreen())
            //{
            //    game.Run();
            //}
        }
    }
}