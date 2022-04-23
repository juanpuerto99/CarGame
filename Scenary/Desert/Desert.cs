using CarGame.Scenary.Desert;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Scenary.Desert
{
    public class Desert : Scenary
    {
        bool parallax1, parallax2, parallax3;
        TimeSpan cactusDelay;
        TimeSpan tumbleWeedDelay;
        TimeSpan nextTumbleWeedDelay;

        DesertStorm desertStorm;
        Random random = new Random();

        public override void LoadContent(ContentManager Content)
        {
            General.TerrainSpriteSheet = Content.Load<Texture2D>("Terrain/Desert");

            Terrains.Add(Terrain.eTerrainType.Ground, new List<Terrain>());
            Color[] c = new Color[1280 * 224];
            General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 464, 1280, 224), c, 0, c.Length);
            Texture2D t = new Texture2D(General.GraphicsDevice, 1280, 224);
            t.SetData(c);
            Terrains[Terrain.eTerrainType.Ground].Add(new Terrain(t, Terrain.eTerrainType.Ground, 0, 360 - 224));

            Terrains.Add(Terrain.eTerrainType.Front1, new List<Terrain>());
            Texture2D t2 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 688, 1280, 48));
            Terrains[Terrain.eTerrainType.Front1].Add(new Terrain(t2, Terrain.eTerrainType.Front1, 0, 360 - 32));

            Terrains.Add(Terrain.eTerrainType.Back1, new List<Terrain>());
            c = new Color[720 * 144];
            General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 0, 720, 144), c, 0, c.Length);
            Texture2D t3 = new Texture2D(General.GraphicsDevice, 720, 144);
            t3.SetData(c);
            Terrain terrain3 = new Terrain(t3, Terrain.eTerrainType.Back1, 0, 140);
            terrain3.Origin = new Vector2(0f, 144);
            Terrains[Terrain.eTerrainType.Back1].Add(terrain3);

            Terrains.Add(Terrain.eTerrainType.Back2, new List<Terrain>());
            Texture2D t4 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 192, 448, 96));
            Terrain terrain4 = new Terrain(t4, Terrain.eTerrainType.Back2, 0, 100);
            terrain4.Origin = new Vector2(0f, 96);
            Terrains[Terrain.eTerrainType.Back2].Add(terrain4);

            Terrains.Add(Terrain.eTerrainType.Back3, new List<Terrain>());
            Texture2D t5 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 304, 304, 96));
            Terrain terrain5 = new Terrain(t5, Terrain.eTerrainType.Back3, 0, 82);
            terrain5.Origin = new Vector2(0f, 96);
            Terrains[Terrain.eTerrainType.Back3].Add(terrain5);

            desertStorm = new DesertStorm(General.TerrainSpriteSheet.GetTexture(new Rectangle(1408, 0, 640, 368)));

            nextTumbleWeedDelay = TimeSpan.FromSeconds(random.Next(5, 15));
        }

        public override void Update(GameTime gameTime)
        {
            //Ground
            float right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Ground].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Ground][i].Update();
                if (!Terrains[Terrain.eTerrainType.Ground][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Ground][i].virtualX + General.Terrains[Terrain.eTerrainType.Ground][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Ground].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640)
            {
                Color[] c = new Color[1280 * 224];
                General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 464, 1280, 224), c, 0, c.Length);
                Texture2D t = new Texture2D(General.GraphicsDevice, 1280, 224);
                t.SetData(c);
                Terrain tn = new Terrain(t, Terrain.eTerrainType.Ground, right, 360 - 224);
                tn.Speed = General.Speed;
                Terrains[Terrain.eTerrainType.Ground].Add(tn);
            }

            //Front
            right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Front1].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Front1][i].Update();
                if (!Terrains[Terrain.eTerrainType.Front1][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Front1][i].virtualX + Terrains[Terrain.eTerrainType.Front1][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Front1].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640)
            {
                Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 688, 1280, 48));
                Terrain tn = new Terrain(t, Terrain.eTerrainType.Front1, right, 360 - 32);
                tn.Speed = General.Speed * 1.3f;
                Terrains[Terrain.eTerrainType.Front1].Add(tn);
            }

            //Background 1
            right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Back1].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Back1][i].Update();
                if (!Terrains[Terrain.eTerrainType.Back1][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Back1][i].virtualX + General.Terrains[Terrain.eTerrainType.Back1][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Back1].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640 + 20)
            {
                if (parallax1)
                {
                    Color[] c = new Color[720 * 144];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 0, 720, 144), c, 0, c.Length);
                    Texture2D t = new Texture2D(General.GraphicsDevice, 720, 144);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back1, right + 20, 140);
                    tn.Speed = General.Speed * 0.7f;
                    tn.Origin = new Vector2(0, 144);
                    Terrains[Terrain.eTerrainType.Back1].Add(tn);
                }
                else
                {
                    Color[] c = new Color[528 * 96];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(720, 32, 528, 96), c, 0, c.Length);
                    Texture2D t = new Texture2D(General.GraphicsDevice, 528, 96);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back1, right + 20, 140);
                    tn.Speed = General.Speed * 0.7f;
                    tn.Origin = new Vector2(0, 96);
                    Terrains[Terrain.eTerrainType.Back1].Add(tn);
                }

                parallax1 = !parallax1;
            }

            //Background 2
            right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Back2].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Back2][i].Update();
                if (!Terrains[Terrain.eTerrainType.Back2][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Back2][i].virtualX + General.Terrains[Terrain.eTerrainType.Back2][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Back2].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640 + 40)
            {
                if (parallax2)
                {
                    Color[] c = new Color[448 * 96];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 192, 448, 96), c, 0, c.Length);
                    Texture2D t = new Texture2D(General.GraphicsDevice, 448, 96);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back2, right + 40, 120);
                    tn.Speed = General.Speed * 0.5f;
                    tn.Origin = new Vector2(0, 96);
                    Terrains[Terrain.eTerrainType.Back2].Add(tn);
                }
                else
                {
                    Color[] c = new Color[400 * 144];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(448, 144, 400, 144), c, 0, c.Length);
                    Texture2D t = new Texture2D(General.GraphicsDevice, 400, 144);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back2, right + 40, 120);
                    tn.Speed = General.Speed * 0.5f;
                    tn.Origin = new Vector2(0, 144);
                    Terrains[Terrain.eTerrainType.Back2].Add(tn);
                }

                parallax2 = !parallax2;
            }

            //Background 3
            right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Back3].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Back3][i].Update();
                if (!Terrains[Terrain.eTerrainType.Back3][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Back3][i].virtualX + Terrains[Terrain.eTerrainType.Back3][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Back3].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640 + 40)
            {
                if (parallax3)
                {
                    Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 304, 304, 96));
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back3, right + 40, 82);
                    tn.Speed = General.Speed * 0.25f;
                    tn.Origin = new Vector2(0, 96);
                    Terrains[Terrain.eTerrainType.Back3].Add(tn);
                }
                else
                {
                    Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(304, 304, 288, 96));
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back3, right + 40, 82);
                    tn.Speed = General.Speed * 0.25f;
                    tn.Origin = new Vector2(0, 96);
                    Terrains[Terrain.eTerrainType.Back3].Add(tn);
                }

                parallax3 = !parallax3;
            }

            CactusUpdate(gameTime);
            TubleWeedUpdate(gameTime);

            for (int i = 0; i < Obstacles.Count; i++)
            {
                Obstacles[i].Update(gameTime);
                if (Obstacles[i].IsDisposed)
                {
                    Obstacles.RemoveAt(i);
                    i--;
                }
            }

            desertStorm.Update();
        }
        public void CactusUpdate(GameTime gameTime)
        {
            cactusDelay += gameTime.ElapsedGameTime;

            if (cactusDelay.TotalSeconds > 1)
            {
                cactusDelay = new TimeSpan();



                int textureIndex = random.Next(0, 6);
                //Texture2D texture = null;
                //if (textureIndex == 0)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 1056, 48, 96));
                //else if (textureIndex == 1)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(48, 1056, 48, 96));
                //else if (textureIndex == 2)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(96, 1056, 64, 96));
                //else if (textureIndex == 3)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(160, 1056, 64, 96));
                //else if (textureIndex == 4)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(224, 1056, 64, 96));
                //else if (textureIndex == 5)
                //    texture = General.TerrainSpriteSheet.GetTexture(new Rectangle(288, 1056, 64, 96));

                Vector2 pos = new Vector2(640 + 50, random.Next(214 + 20, 360));
                Cactus cactus = new Cactus(General.TerrainSpriteSheet, pos, textureIndex);

                Obstacles.Add(cactus);
                Console.WriteLine("Cactus añadido!!");
            }
        }
        public void TubleWeedUpdate(GameTime gameTime)
        {
            tumbleWeedDelay += gameTime.ElapsedGameTime;

            if (tumbleWeedDelay > nextTumbleWeedDelay)
            {
                tumbleWeedDelay = new TimeSpan();
                nextTumbleWeedDelay = TimeSpan.FromSeconds(random.Next(1, 2));

                Vector2 pos = new Vector2(640 + 50, random.Next(214 + 20, 350));
                TumbleWeed tumbleWeed = new TumbleWeed(pos);
                tumbleWeed.Direction = Convert.ToBoolean(random.Next(0, 1));
                Obstacles.Add(tumbleWeed);
            }
        }

        public override void Draw(GameTime gameTime)
        {

        }
        public override void PostDraw(GameTime gameTime)
        {
            General.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointWrap);
            //desertStorm.Draw(General.SpriteBatch);
            General.SpriteBatch.End();
        }
    }
}
