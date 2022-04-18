using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        Rectangle groundRectangle = new Rectangle(0, 360 - 224, 1280, 224);
        bool parallax1, parallax2, parallax3;

        public class Sprite
        {
            public Rectangle rec;
            public Texture2D texture;
            public float positionProgression = 0f;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        protected override void LoadContent()
        {
            this.graphics.PreferredBackBufferWidth = 1600;
            this.graphics.PreferredBackBufferHeight = 900;
            //this.graphics.PreferredBackBufferWidth = 4 * 300;
            //this.graphics.PreferredBackBufferHeight = 3 * 300;
            this.graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360);
            //screenRender = new RenderTarget2D(GraphicsDevice, 640, 480);

            General.TerrainSpriteSheet = Content.Load<Texture2D>("Terrain/Desert");
            General.Pixel = new Texture2D(GraphicsDevice, 1, 1);
            General.Pixel.SetData(new Color[1] { Color.White });

            General.Terrains.Add(Terrain.eTerrainType.Ground, new List<Terrain>());
            Color[] c = new Color[1280 * 224];
            General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 464, 1280, 224), c, 0, c.Length);
            Texture2D t = new Texture2D(GraphicsDevice, 1280, 224);
            t.SetData(c);
            General.Terrains[Terrain.eTerrainType.Ground].Add(new Terrain(t, Terrain.eTerrainType.Ground, 0, 360 - 224));

            General.Terrains.Add(Terrain.eTerrainType.Front1, new List<Terrain>());
            c = new Color[1280 * 48];
            General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 752, 1280, 48), c, 0, c.Length);
            Texture2D t2 = new Texture2D(GraphicsDevice, 1280, 48);
            t2.SetData(c);
            General.Terrains[Terrain.eTerrainType.Front1].Add(new Terrain(t2, Terrain.eTerrainType.Front1, 0, 360 - 32));

            General.Terrains.Add(Terrain.eTerrainType.Back1, new List<Terrain>());
            c = new Color[720 * 144];
            General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 0, 720, 144), c, 0, c.Length);
            Texture2D t3 = new Texture2D(GraphicsDevice, 720, 144);
            t3.SetData(c);
            Terrain terrain3 = new Terrain(t3, Terrain.eTerrainType.Back1, 0, 140);
            terrain3.Origin = new Vector2(0f, 144);
            General.Terrains[Terrain.eTerrainType.Back1].Add(terrain3);

            General.Terrains.Add(Terrain.eTerrainType.Back2, new List<Terrain>());
            Texture2D t4 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 192, 448, 96));
            Terrain terrain4 = new Terrain(t4, Terrain.eTerrainType.Back2, 0, 100);
            terrain4.Origin = new Vector2(0f, 96);
            General.Terrains[Terrain.eTerrainType.Back2].Add(terrain4);

            General.Terrains.Add(Terrain.eTerrainType.Back3, new List<Terrain>());
            Texture2D t5 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 304, 304, 96));
            Terrain terrain5 = new Terrain(t5, Terrain.eTerrainType.Back3, 0, 82);
            terrain5.Origin = new Vector2(0f, 96);
            General.Terrains[Terrain.eTerrainType.Back3].Add(terrain5);

            General.Car = new Car();

            General.Speed = 0;
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (General.Speed < 3)
            {
                General.Speed = Math.Min(General.Speed + 0.01f, 3);
                for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Ground].Count; i++)
                    General.Terrains[Terrain.eTerrainType.Ground][i].Speed = General.Speed;

                for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Front1].Count; i++)
                    General.Terrains[Terrain.eTerrainType.Front1][i].Speed = General.Speed * 1.3f;

                for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back1].Count; i++)
                    General.Terrains[Terrain.eTerrainType.Back1][i].Speed = General.Speed * 0.7f;

                for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back2].Count; i++)
                    General.Terrains[Terrain.eTerrainType.Back2][i].Speed = General.Speed * 0.5f;

                for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back3].Count; i++)
                    General.Terrains[Terrain.eTerrainType.Back3][i].Speed = General.Speed * 0.25f;
            }

            //Ground
            float right = -1;
            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Ground].Count; i++)
            {
                General.Terrains[Terrain.eTerrainType.Ground][i].Update();
                if (!General.Terrains[Terrain.eTerrainType.Ground][i].isDisposed)
                    right = Math.Max(right, General.Terrains[Terrain.eTerrainType.Ground][i].virtualX + General.Terrains[Terrain.eTerrainType.Ground][i].Zone.Width);
                else
                {
                    General.Terrains[Terrain.eTerrainType.Ground].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640)
            {
                Color[] c = new Color[1280 * 224];
                General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 464, 1280, 224), c, 0, c.Length);
                Texture2D t = new Texture2D(GraphicsDevice, 1280, 224);
                t.SetData(c);
                Terrain tn = new Terrain(t, Terrain.eTerrainType.Ground, right, 360 - 224);
                tn.Speed = General.Speed;
                General.Terrains[Terrain.eTerrainType.Ground].Add(tn);
            }

            //Front
            right = -1;
            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Front1].Count; i++)
            {
                General.Terrains[Terrain.eTerrainType.Front1][i].Update();
                if (!General.Terrains[Terrain.eTerrainType.Front1][i].isDisposed)
                    right = Math.Max(right, General.Terrains[Terrain.eTerrainType.Front1][i].virtualX + General.Terrains[Terrain.eTerrainType.Front1][i].Zone.Width);
                else
                {
                    General.Terrains[Terrain.eTerrainType.Front1].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640)
            {
                Color[] c = new Color[1280 * 48];
                General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 752, 1280, 48), c, 0, c.Length);
                Texture2D t = new Texture2D(GraphicsDevice, 1280, 48);
                t.SetData(c);
                Terrain tn = new Terrain(t, Terrain.eTerrainType.Front1, right, 360 - 32);
                tn.Speed = General.Speed * 1.3f;
                General.Terrains[Terrain.eTerrainType.Front1].Add(tn);
            }

            //Background 1
            right = -1;
            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back1].Count; i++)
            {
                General.Terrains[Terrain.eTerrainType.Back1][i].Update();
                if (!General.Terrains[Terrain.eTerrainType.Back1][i].isDisposed)
                    right = Math.Max(right, General.Terrains[Terrain.eTerrainType.Back1][i].virtualX + General.Terrains[Terrain.eTerrainType.Back1][i].Zone.Width);
                else
                {
                    General.Terrains[Terrain.eTerrainType.Back1].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640 + 20)
            {
                if (parallax1)
                {
                    Color[] c = new Color[720 * 144];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 0, 720, 144), c, 0, c.Length);
                    Texture2D t = new Texture2D(GraphicsDevice, 720, 144);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back1, right + 20, 140);
                    tn.Speed = General.Speed * 0.7f;
                    tn.Origin = new Vector2(0, 144);
                    General.Terrains[Terrain.eTerrainType.Back1].Add(tn);
                }
                else
                {
                    Color[] c = new Color[528 * 96];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(720, 32, 528, 96), c, 0, c.Length);
                    Texture2D t = new Texture2D(GraphicsDevice, 528, 96);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back1, right + 20, 140);
                    tn.Speed = General.Speed * 0.7f;
                    tn.Origin = new Vector2(0, 96);
                    General.Terrains[Terrain.eTerrainType.Back1].Add(tn);
                }

                parallax1 = !parallax1;
            }

            //Background 2
            right = -1;
            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back2].Count; i++)
            {
                General.Terrains[Terrain.eTerrainType.Back2][i].Update();
                if (!General.Terrains[Terrain.eTerrainType.Back2][i].isDisposed)
                    right = Math.Max(right, General.Terrains[Terrain.eTerrainType.Back2][i].virtualX + General.Terrains[Terrain.eTerrainType.Back2][i].Zone.Width);
                else
                {
                    General.Terrains[Terrain.eTerrainType.Back2].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640 + 40)
            {
                if (parallax2)
                {
                    Color[] c = new Color[448 * 96];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 192, 448, 96), c, 0, c.Length);
                    Texture2D t = new Texture2D(GraphicsDevice, 448, 96);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back2, right + 40, 120);
                    tn.Speed = General.Speed * 0.5f;
                    tn.Origin = new Vector2(0, 96);
                    General.Terrains[Terrain.eTerrainType.Back2].Add(tn);
                }
                else
                {
                    Color[] c = new Color[400 * 144];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(448, 144, 400, 144), c, 0, c.Length);
                    Texture2D t = new Texture2D(GraphicsDevice, 400, 144);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back2, right + 40, 120);
                    tn.Speed = General.Speed * 0.5f;
                    tn.Origin = new Vector2(0, 144);
                    General.Terrains[Terrain.eTerrainType.Back2].Add(tn);
                }

                parallax2 = !parallax2;
            }

            //Background 3
            right = -1;
            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back3].Count; i++)
            {
                General.Terrains[Terrain.eTerrainType.Back3][i].Update();
                if (!General.Terrains[Terrain.eTerrainType.Back3][i].isDisposed)
                    right = Math.Max(right, General.Terrains[Terrain.eTerrainType.Back3][i].virtualX + General.Terrains[Terrain.eTerrainType.Back3][i].Zone.Width);
                else
                {
                    General.Terrains[Terrain.eTerrainType.Back3].RemoveAt(i);
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
                    General.Terrains[Terrain.eTerrainType.Back3].Add(tn);
                }
                else
                {
                    Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(304, 304, 288, 96));
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Back3, right + 40, 82);
                    tn.Speed = General.Speed * 0.25f;
                    tn.Origin = new Vector2(0, 96);
                    General.Terrains[Terrain.eTerrainType.Back3].Add(tn);
                }

                parallax3 = !parallax3;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(0, -120, 720, 288), new Rectangle(0, 832, 720, 288), Color.White);

            spriteBatch.Draw(General.Pixel, new Rectangle(0, 65, 640, 86), new Color(255, 213, 65));

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back3].Count; i++)
                General.Terrains[Terrain.eTerrainType.Back3][i].Draw(spriteBatch);

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back2].Count; i++)
                General.Terrains[Terrain.eTerrainType.Back2][i].Draw(spriteBatch);

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Back1].Count; i++)
                General.Terrains[Terrain.eTerrainType.Back1][i].Draw(spriteBatch);

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Ground].Count; i++)
                General.Terrains[Terrain.eTerrainType.Ground][i].Draw(spriteBatch);

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Front1].Count; i++)
                General.Terrains[Terrain.eTerrainType.Front1][i].Draw(spriteBatch);

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
