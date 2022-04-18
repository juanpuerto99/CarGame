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
        public static Car Car;
        public static Dictionary<Terrain.eTerrainType, List<Terrain>> Terrains = new Dictionary<Terrain.eTerrainType, List<Terrain>>();
        public static Texture2D Pixel;

        public static float Speed = 2f;
        public static Texture2D TerrainSpriteSheet; 
    }
}
