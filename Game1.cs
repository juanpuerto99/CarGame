using CarGame.Scenary;
using CarGame.Scenary.Desert;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

//https://mysteriousspace.com/2019/01/05/pixel-shaders-in-monogame-a-tutorial-of-sorts-for-2019/
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

            General.GraphicsDevice = GraphicsDevice;
            General.SpriteBatch = spriteBatch;
            General.Pixel = new Texture2D(GraphicsDevice, 1, 1);
            General.Pixel.SetData(new Color[1] { Color.White });

            General.Car = new Car();
            General.ScenaryManager = new Desert();
            General.ScenaryManager.LoadContent(Content);
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

            General.ScenaryManager.Update(gameTime);

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

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            for (int i = 0; i < General.Obstacles.Count; i++)
                General.Obstacles[i].Draw(gameTime);

            //for (int i = 0; i < General.Obstacles.Count; i++)
            //    General.Obstacles[i].DrawHitBox(gameTime);

            spriteBatch.End();
            spriteBatch.Begin();

            for (int i = 0; i < General.Terrains[Terrain.eTerrainType.Front1].Count; i++)
                General.Terrains[Terrain.eTerrainType.Front1][i].Draw(spriteBatch);

            //spriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(100, 100, 48, 96), new Rectangle(0, 1056, 48, 96), Color.White);
            spriteBatch.End();

            General.ScenaryManager.PostDraw(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
