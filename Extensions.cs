using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public static class Extensions
    {
        public static Texture2D GetTexture(this Texture2D texture, Rectangle source)
        {
            Color[] c = new Color[source.Width * source.Height];
            General.TerrainSpriteSheet.GetData(0, source, c, 0, c.Length);
            Texture2D t = new Texture2D(texture.GraphicsDevice, source.Width, source.Height);
            t.SetData(c);
            return t;
        }
    }
}
