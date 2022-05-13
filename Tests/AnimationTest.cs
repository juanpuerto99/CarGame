using CarGame.Scenary.Desert;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Tests
{
    public class AnimationTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;
        Spike spike1;
        Spike spike2;
        Spike spike3;

        public AnimationTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsFixedTimeStep = true;
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.graphics.PreferredBackBufferWidth = 640 * 2;
            this.graphics.PreferredBackBufferHeight = 360 * 2;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360, false, SurfaceFormat.Color, DepthFormat.Depth24);

            General.TerrainSpriteSheet = Content.Load<Texture2D>("Terrain/Desert");
            General.SpriteBatch = spriteBatch;
            General.GraphicsDevice = GraphicsDevice;
            General.Speed = 0f;

            spike1 = new Spike(General.TerrainSpriteSheet, 0, new Vector2(10 + 64 * 0, 10));
            spike2 = new Spike(General.TerrainSpriteSheet, 1, new Vector2(10 + 64 * 1, 10));
            spike3 = new Spike(General.TerrainSpriteSheet, 2, new Vector2(10 + 64 * 2, 10));
        }
        protected override void Update(GameTime gameTime)
        {
            General.GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            spike1.Update(gameTime);
            spike2.Update(gameTime);
            spike3.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spike1.Draw(gameTime);
            spike2.Draw(gameTime);
            spike3.Draw(gameTime);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            //Render on Windows Scale
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
