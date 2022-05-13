using CarGame.Scenary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public static class General
    {
        public static GraphicsDevice GraphicsDevice;
        public static SpriteBatch SpriteBatch;
        public static GameTime GameTime;

        public static Car Car;

        public static Scenary.Scenary ScenaryManager;
        public static Dictionary<Terrain.eTerrainType, List<Terrain>> Terrains { get => ScenaryManager.Terrains; }
        public static List<Entity> Obstacles { get => ScenaryManager.Obstacles; }
        public static Texture2D Pixel;

        public static float Speed = 2f;
        public static Texture2D TerrainSpriteSheet;

        public static Random Random;

        public static float DeltaTime = 1f;
    }
}
