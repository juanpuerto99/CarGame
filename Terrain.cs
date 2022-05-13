using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public class Terrain
    {
        public Texture2D Texture;
        public Rectangle Zone;
        public float virtualX;
        Rectangle source;
        public bool isDisposed = false;
        public float Speed;
        public Vector2 Origin;

        public eTerrainType TerrainType = eTerrainType.Ground;
        public enum eTerrainType
        {
            Back1,
            Back2,
            Back3,
            Ground,
            Front1,
            Front2,
            Front3,
        }

        public Terrain(Rectangle sourcee, eTerrainType TerrainType, float x, float y)
        {

        }
        public Terrain(Texture2D texture, eTerrainType TerrainType, float x, float y)
        {
            this.Texture = texture;
            this.TerrainType = TerrainType;
            virtualX = x;
            Zone = new Rectangle((int)Math.Floor(x), (int)Math.Floor(y), texture.Width, texture.Height);
            Origin = Vector2.Zero;
        }
        public void Update()
        {
            if (isDisposed) return;

            virtualX -= Speed;
            Zone.X = (int)Math.Floor(virtualX);

            if (Zone.Right < 0)
            {
                Dispose();
                return;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDisposed) return;
            spriteBatch.Draw(Texture, Zone, null, Color.White, 0f, Origin, SpriteEffects.None, 1f);
        }
        public void Dispose()
        {
            Texture.Dispose();
            Texture = null;
            isDisposed = true;
        }
    }
}
