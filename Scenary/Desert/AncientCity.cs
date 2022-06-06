using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Scenary.Desert
{
    public class AncientCity : Scenary
    {
        public override void LoadContent(ContentManager Content)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {

        }

        private Texture2D CreateRuinsFloor(GameTime gameTime)
        {
            //int qX = 1280 / 41;
            //int qY = 176 / 24;

            RenderTarget2D floor = new RenderTarget2D(General.GraphicsDevice, 1280, 176);
            General.GraphicsDevice.SetRenderTarget(floor);
            General.SpriteBatch.Begin();
            for (int y = 176; y >= 0; y++)
            {
                int acumX = 0;
                for (int x = 0; x < 1280; x++)
                {
                    Rectangle source;
                    int slabIndex = General.Random.Next(0, 10);
                    if (slabIndex < 6) source = new Rectangle(64, 1379, 64, 32);
                    else if (slabIndex < 7) source = new Rectangle(128, 1379, 64, 32);
                    else if (slabIndex < 8) source = new Rectangle(192, 1379, 64, 32);
                    else if (slabIndex < 9) source = new Rectangle(64, 1408, 64, 32);
                    else if (slabIndex < 10) source = new Rectangle(128, 1408, 64, 32);
                    else source = new Rectangle(192, 1408, 64, 32);

                    General.SpriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(x, y, 64, 32), source, Color.White);
                    acumX += 41;
                }
            }
            General.SpriteBatch.End();
            General.GraphicsDevice.SetRenderTarget(null);
            return floor;
        }
    }
}
