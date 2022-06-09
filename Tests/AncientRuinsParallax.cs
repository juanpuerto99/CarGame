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
    public class AncientRuinsParallax : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        Texture2D desertSpriteSheet;

        Rectangle skySource = new Rectangle(0, 752, 720, 288);
        Rectangle cloudsSource = new Rectangle(720, 816, 720, 48);
        Rectangle parallax1Source = new Rectangle(0, 1632, 2048, 128);
        Rectangle parallax2Source = new Rectangle(0, 1760, 1024, 64);
        Rectangle parallax3Source = new Rectangle(0, 1824, 512, 32);
        Rectangle backgroundSource = new Rectangle(1424, 1152, 256, 224);
        Rectangle frontgroundSource = new Rectangle(256, 1152, 256, 224);

        int cloudY = 30;
        int parallax1Y = 154;
        int parallax2Y = 112;
        int parallax3Y = 79;
        int backgroundY = 210;
        int frontgroundY = 392;

        float cloudMov = 0;
        float parallax1Mov = 0;
        float parallax2Mov = 0;
        float parallax3Mov = 0;
        float backgroundMov = 0;
        float frontgroundMov = 0;

        Vector2 parallax1Origin;
        Vector2 parallax2Origin;
        Vector2 parallax3Origin;
        Vector2 backgroundOrigin;
        Vector2 frontgroundOrigin;

        Sprite skySprite;
        List<Sprite> cloudSprite = new List<Sprite>();

        List<Sprite> parallax1Sprites = new List<Sprite>();
        List<Sprite> parallax2Sprites = new List<Sprite>();
        List<Sprite> parallax3Sprites = new List<Sprite>();

        List<Sprite> floorSprites;
        List<Sprite> backgroundSprites;
        List<Sprite> frontgroundSprites;

        public AncientRuinsParallax()
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
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360);

            General.SpriteBatch = spriteBatch;
            General.GraphicsDevice = GraphicsDevice;

            desertSpriteSheet = Content.Load<Texture2D>("Terrain/Desert");
            parallax1Origin = new Vector2(0, parallax1Source.Height);
            parallax2Origin = new Vector2(0, parallax2Source.Height);
            parallax3Origin = new Vector2(0, parallax3Source.Height);
            backgroundOrigin = new Vector2(0, backgroundSource.Height);
            frontgroundOrigin = new Vector2(0, frontgroundSource.Height);

            skySprite = new Sprite(desertSpriteSheet, 0, -136, skySource, Color.White);
            cloudSprite = new List<Sprite>();
            cloudSprite.Add(new Sprite(desertSpriteSheet, 0, cloudY, cloudsSource, Color.White));

            parallax1Sprites = new List<Sprite>();
            parallax1Sprites.Add(new Sprite(desertSpriteSheet, 0, parallax1Y, parallax1Source, Color.White, parallax1Origin));

            parallax2Sprites = new List<Sprite>();
            parallax2Sprites.Add(new Sprite(desertSpriteSheet, 0, parallax2Y, parallax2Source, Color.White, parallax2Origin));

            parallax3Sprites = new List<Sprite>();
            parallax3Sprites.Add(new Sprite(desertSpriteSheet, 0, parallax3Y, parallax3Source, Color.White, parallax3Origin));
            parallax3Sprites.Add(new Sprite(desertSpriteSheet, parallax3Source.Width, parallax3Y, parallax3Source, Color.White, parallax3Origin));

            backgroundSprites = new List<Sprite>();
            backgroundSprites.Add(new Sprite(desertSpriteSheet, 0, backgroundY, backgroundSource, Color.White, backgroundOrigin));
            backgroundSprites.Add(new Sprite(desertSpriteSheet, backgroundSource.Width, backgroundY, backgroundSource, Color.White, backgroundOrigin));
            backgroundSprites.Add(new Sprite(desertSpriteSheet, backgroundSource.Width * 2, backgroundY, backgroundSource, Color.White, backgroundOrigin));

            frontgroundSprites = new List<Sprite>();
            frontgroundSprites.Add(new Sprite(desertSpriteSheet, 0, frontgroundY, frontgroundSource, Color.White, frontgroundOrigin));
            frontgroundSprites.Add(new Sprite(desertSpriteSheet, frontgroundSource.Width, frontgroundY, frontgroundSource, Color.White, frontgroundOrigin));
            frontgroundSprites.Add(new Sprite(desertSpriteSheet, frontgroundSource.Width * 2, frontgroundY, frontgroundSource, Color.White, frontgroundOrigin));

            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            General.GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            UpdateClouds();
            UpdateParallax1();
            UpdateParallax2();
            UpdateParallax3();
            UpdateBackground();
            UpdateFrontground();
        }

        private void UpdateClouds()
        {
            cloudMov += 0.05f;
            int mov = (int)Math.Floor(cloudMov);
            if (mov > 0) cloudMov -= mov;

            int right = 0;
            for (int i = 0; i < cloudSprite.Count; i++)
            {
                cloudSprite[i].X -= mov;
                if (cloudSprite[i].Right < 0) cloudSprite.RemoveAt(i);
                else if (cloudSprite[i].Right > right) right = cloudSprite[i].Right;
            }

            if (right < 640)
                cloudSprite.Add(new Sprite(desertSpriteSheet, right, cloudY, cloudsSource, Color.White));
        }
        private void UpdateParallax1()
        {
            parallax1Mov += 2;
            int mov = (int)Math.Floor(parallax1Mov);
            if (mov > 0) parallax1Mov -= mov;

            int right = 0;
            for (int i = 0; i < parallax1Sprites.Count; i++)
            {
                parallax1Sprites[i].X -= mov;
                if (parallax1Sprites[i].Right < 0)
                {
                    parallax1Sprites.RemoveAt(i);
                    i--;
                }
                else if (parallax1Sprites[i].Right > right) right = parallax1Sprites[i].Right;
            }

            if (right < 640)
                parallax1Sprites.Add(new Sprite(desertSpriteSheet, right, parallax1Y, parallax1Source, Color.White, parallax1Origin));
        }
        private void UpdateParallax2()
        {
            parallax2Mov += 1;
            int mov = (int)Math.Floor(parallax2Mov);
            if (mov > 0) parallax2Mov -= mov;

            int right = 0;
            for (int i = 0; i < parallax2Sprites.Count; i++)
            {
                parallax2Sprites[i].X -= mov;
                if (parallax2Sprites[i].Right < 0)
                {
                    parallax2Sprites.RemoveAt(i);
                    i--;
                }
                else if (parallax2Sprites[i].Right > right) right = parallax2Sprites[i].Right;
            }
            
            if (right < 640)
                parallax2Sprites.Add(new Sprite(desertSpriteSheet, right, parallax2Y, parallax2Source, Color.White, parallax2Origin));
        }
        private void UpdateParallax3()
        {
            parallax3Mov += 0.5f;
            int mov = (int)Math.Floor(parallax3Mov);
            if (mov > 0) parallax3Mov -= mov;

            int right = 0;
            for (int i = 0; i < parallax3Sprites.Count; i++)
            {
                parallax3Sprites[i].X -= mov;
                if (parallax3Sprites[i].Right < 0)
                {
                    parallax3Sprites.RemoveAt(i);
                    i--;
                }
                else if (parallax3Sprites[i].Right > right) right = parallax3Sprites[i].Right;
            }

            if (right < 640)
                parallax3Sprites.Add(new Sprite(desertSpriteSheet, right, parallax3Y, parallax3Source, Color.White, parallax3Origin));
        }
        private void UpdateBackground()
        {
            backgroundMov += 3f;
            int mov = (int)Math.Floor(backgroundMov);
            if (mov > 0) backgroundMov -= mov;

            int right = 0;
            for (int i = 0; i < backgroundSprites.Count; i++)
            {
                backgroundSprites[i].X -= mov;
                if (backgroundSprites[i].Right < 0)
                {
                    backgroundSprites.RemoveAt(i);
                    i--;
                }
                else if (backgroundSprites[i].Right > right) right = backgroundSprites[i].Right;
            }

            if (right < 640)
                backgroundSprites.Add(new Sprite(desertSpriteSheet, right, backgroundY, backgroundSource, Color.White, backgroundOrigin));
        }
        private void UpdateFrontground()
        {
            frontgroundMov += 4f;
            int mov = (int)Math.Floor(frontgroundMov);
            if (mov > 0) frontgroundMov -= mov;

            int right = 0;
            for (int i = 0; i < frontgroundSprites.Count; i++)
            {
                frontgroundSprites[i].X -= mov;
                if (frontgroundSprites[i].Right < 0)
                {
                    frontgroundSprites.RemoveAt(i);
                    i--;
                }
                else if (frontgroundSprites[i].Right > right) right = frontgroundSprites[i].Right;
            }

            if (right < 640)
                frontgroundSprites.Add(new Sprite(desertSpriteSheet, right, frontgroundY, frontgroundSource, Color.White, frontgroundOrigin));
        }

        protected override void Draw(GameTime gameTime)
        {
            //Render on 640x360 px
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            skySprite.Draw();
            foreach (Sprite cs in cloudSprite) cs.Draw();
            foreach (Sprite ps in parallax3Sprites) ps.Draw();
            foreach (Sprite ps in parallax2Sprites) ps.Draw();
            foreach (Sprite ps in parallax1Sprites) ps.Draw();
            foreach (Sprite bs in backgroundSprites) bs.Draw();
            foreach (Sprite bs in frontgroundSprites) bs.Draw();

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();
        }
    }
}
