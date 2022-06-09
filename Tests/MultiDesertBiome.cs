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
    public class MultiDesertBiome : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        SpriteFont font;
        string debugInfo;

        Texture2D desertSpriteSheet;
        Scenary scenary;

        BiomeType biome;
        BiomeType previousParallaxBiome;
        BiomeType parallaxBiome;
        bool interpolingParallax = false;
        float parallaxInterpol = 0;
        float interpolSpeed = 0.02f;

        int parallax1Y = 154;
        int parallax2Y = 112;
        int parallax3Y = 79;
        int dunesGroundY = 146;
        int ancientRuinsGroundY = -20;
        int cloudY = 30;

        //Rectangle skySource = new Rectangle(0, 752, 720, 288);
        //Rectangle cloudsSource = new Rectangle(720, 816, 720, 48);
        //Rectangle dunesParallax1Source = new Rectangle(0, 1632, 2048, 128);
        //Rectangle dunesParallax2Source = new Rectangle(0, 1760, 1024, 64);
        //Rectangle dunesParallax3Source = new Rectangle(0, 1824, 512, 32);
        //Rectangle ancientRuinsParallax1Source = new Rectangle(0, 1632, 2048, 128);
        //Rectangle ancientRuinsParallax2Source = new Rectangle(0, 1760, 1024, 64);
        //Rectangle ancientRuinsParallax3Source = new Rectangle(0, 1824, 512, 32);
        //Rectangle dunesGroundSource = new Rectangle(0, 464, 1280, 224);
        Rectangle skySource = new Rectangle(0, 1760, 720, 288);
        Rectangle cloudsSource = new Rectangle(0, 1712, 720, 48);
        Rectangle dunesParallax1Source = new Rectangle(0, 224, 2048, 112);
        Rectangle dunesParallax2Source = new Rectangle(0, 336, 1024, 80);
        Rectangle dunesParallax3Source = new Rectangle(0, 416, 512, 32);
        Rectangle ancientRuinsParallax1Source = new Rectangle(0, 0, 2048, 128);
        Rectangle ancientRuinsParallax2Source = new Rectangle(0, 128, 1024, 64);
        Rectangle ancientRuinsParallax3Source = new Rectangle(0, 192, 512, 32);

        Rectangle ancientCityFront1GroundSource = new Rectangle(0, 736, 256, 224);
        Rectangle ancientCityFront2GroundSource = new Rectangle(256, 736, 256, 224);
        Rectangle ancientCityFront3GroundSource = new Rectangle(512, 736, 256, 224);
        Rectangle ancientCityBack1GroundSource = new Rectangle(1152, 736, 256, 224);
        Rectangle ancientCityBack2GroundSource = new Rectangle(256, 736, 256, 224);
        Rectangle ancientCityBack3GroundSource = new Rectangle(1424, 736, 256, 224);
        Rectangle dunesGroundSource = new Rectangle(0, 464, 1280, 224);

        Vector2 ancientRuinsParallax1Origin;
        Vector2 ancientRuinsParallax2Origin;
        Vector2 ancientRuinsParallax3Origin;

        Vector2 dunesParallax1Origin;
        Vector2 dunesParallax2Origin;
        Vector2 dunesParallax3Origin;

        float parallax1Mov = 0f;
        float parallax2Mov = 0f;
        float parallax3Mov = 0f;
        float groundMov = 0f;
        float cloudMov = 0f;

        Sprite skySprite;
        List<Sprite> cloudSprite = new List<Sprite>();

        public MultiDesertBiome()
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

            font = Content.Load<SpriteFont>("Fonts/Arial12");

            General.SpriteBatch = spriteBatch;
            General.GraphicsDevice = GraphicsDevice;
            General.Random = new Random();

            //desertSpriteSheet = Content.Load<Texture2D>("Terrain/Desert");
            desertSpriteSheet = Content.Load<Texture2D>("Textures/Terrain/Desert");

            dunesParallax1Origin = new Vector2(0, dunesParallax1Source.Height);
            dunesParallax2Origin = new Vector2(0, dunesParallax2Source.Height);
            dunesParallax3Origin = new Vector2(0, dunesParallax3Source.Height);
            ancientRuinsParallax1Origin = new Vector2(0, ancientRuinsParallax1Source.Height);
            ancientRuinsParallax2Origin = new Vector2(0, ancientRuinsParallax2Source.Height);
            ancientRuinsParallax3Origin = new Vector2(0, ancientRuinsParallax3Source.Height);

            parallaxBiome = BiomeType.Dunes;

            scenary = new Scenary();
            scenary.Obstacles = new List<Entity>();

            scenary.Terrains = new Dictionary<TerrainType, List<Terrain>>();
            scenary.Terrains.Add(TerrainType.Parrallax1, new List<Terrain>());
            scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax1Y, dunesParallax1Source, dunesParallax1Origin));
            scenary.Terrains.Add(TerrainType.Parrallax2, new List<Terrain>());
            scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax2Y, dunesParallax2Source, dunesParallax2Origin));
            scenary.Terrains.Add(TerrainType.Parrallax3, new List<Terrain>());
            scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax3Y, dunesParallax3Source, dunesParallax3Origin));
            scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, dunesParallax3Source.X, parallax3Y, dunesParallax3Source, dunesParallax3Origin));
            scenary.Terrains.Add(TerrainType.Ground, new List<Terrain>());

            skySprite = new Sprite(desertSpriteSheet, 0, -136, skySource, Color.White);
            skySprite.Depth = 0;
            cloudSprite = new List<Sprite>();
            cloudSprite.Add(new Sprite(desertSpriteSheet, 0, cloudY, cloudsSource, Color.White));

        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            General.GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.O))
            {
                if (biome != BiomeType.Dunes) biome = BiomeType.Dunes;
            }
            if (ks.IsKeyDown(Keys.P))
            {
                if (biome != BiomeType.AncientRuins) biome = BiomeType.AncientRuins;
            }

            UpdateParallax();
            UpdateGround();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Biome: " + biome.ToString());
            sb.AppendLine("Parallax Biome: " + parallaxBiome.ToString());
            sb.AppendLine("Prev Parallax Biome: " + previousParallaxBiome.ToString());
            debugInfo = sb.ToString();
        }
        private void UpdateParallax()
        {
            UpdateParallaxBiomeChange();
            UpdateParallax1();
            UpdateParallax2();
            UpdateParallax3();
        }
        private void UpdateParallaxBiomeChange()
        {
            if (scenary.Terrains[TerrainType.Ground].Count > 1 &&
                scenary.Terrains[TerrainType.Ground][1].BiomeType != (byte)parallaxBiome &&
                scenary.Terrains[TerrainType.Ground][1].X < 320)
            {
                //Interpol parallax
                previousParallaxBiome = parallaxBiome;
                parallaxBiome = (BiomeType)scenary.Terrains[TerrainType.Ground][1].BiomeType;
                interpolingParallax = true;

                //Prepare to delete obsolete parallax
                for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax1].Count; i++)
                {
                    scenary.Terrains[TerrainType.Parrallax1][i].Obsolete = true;
                    scenary.Terrains[TerrainType.Parrallax1][i].Depth = 1f;
                }

                for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax2].Count; i++)
                {
                    scenary.Terrains[TerrainType.Parrallax2][i].Obsolete = true;
                    scenary.Terrains[TerrainType.Parrallax2][i].Depth = 1f;
                }

                for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax3].Count; i++)
                {
                    scenary.Terrains[TerrainType.Parrallax3][i].Obsolete = true;
                    scenary.Terrains[TerrainType.Parrallax3][i].Depth = 1f;
                }

                //Load new parallax
                if (parallaxBiome == BiomeType.Dunes)
                {
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax1Y, dunesParallax1Source, dunesParallax1Origin));
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax2Y, dunesParallax2Source, dunesParallax2Origin));
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, 0, parallax2Y, dunesParallax3Source, dunesParallax3Origin));
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, dunesParallax3Source.X, parallax3Y, dunesParallax3Source, dunesParallax3Origin));
                }
                else if (parallaxBiome == BiomeType.AncientRuins)
                {
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, 0, parallax1Y, ancientRuinsParallax1Source, ancientRuinsParallax1Origin));
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, 0, parallax2Y, ancientRuinsParallax2Source, ancientRuinsParallax2Origin));
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, 0, parallax2Y, ancientRuinsParallax3Source, ancientRuinsParallax3Origin));
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, ancientRuinsParallax3Source.X, parallax3Y, ancientRuinsParallax3Source, ancientRuinsParallax3Origin));
                }

                parallaxInterpol = 1f;
            }

            if (parallaxInterpol > 0)
            {
                parallaxInterpol -= interpolSpeed * General.DeltaTime;
                if (parallaxInterpol > 0)
                {
                    //Update parallax Alpha
                    byte previousAlpha = Convert.ToByte(255 * parallaxInterpol);
                    byte newAlpha = Convert.ToByte(255 - 255 * parallaxInterpol);
                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax1].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax1][i].Obsolete)
                            scenary.Terrains[TerrainType.Parrallax1][i].Alpha = previousAlpha;
                        else scenary.Terrains[TerrainType.Parrallax1][i].Alpha = newAlpha;

                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax2].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax2][i].Obsolete)
                            scenary.Terrains[TerrainType.Parrallax2][i].Alpha = previousAlpha;
                        else scenary.Terrains[TerrainType.Parrallax2][i].Alpha = newAlpha;

                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax3].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax3][i].Obsolete)
                            scenary.Terrains[TerrainType.Parrallax3][i].Alpha = previousAlpha;
                        else scenary.Terrains[TerrainType.Parrallax3][i].Alpha = newAlpha;
                }
                else
                {
                    //Remove obsolete parallax
                    parallaxInterpol = 0;
                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax1].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax1][i].Obsolete)
                        {
                            scenary.Terrains[TerrainType.Parrallax1].RemoveAt(i);
                            i--;
                        }
                        else scenary.Terrains[TerrainType.Parrallax1][i].Alpha = 255;
                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax2].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax2][i].Obsolete)
                        {
                            scenary.Terrains[TerrainType.Parrallax2].RemoveAt(i);
                            i--;
                        }
                        else scenary.Terrains[TerrainType.Parrallax2][i].Alpha = 255;
                    for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax3].Count; i++)
                        if (scenary.Terrains[TerrainType.Parrallax3][i].Obsolete)
                        {
                            scenary.Terrains[TerrainType.Parrallax3].RemoveAt(i);
                            i--;
                        }
                        else scenary.Terrains[TerrainType.Parrallax3][i].Alpha = 255;
                }
            }
        }
        private void UpdateParallax1()
        {
            parallax1Mov += 2;
            int mov = (int)Math.Floor(parallax1Mov);
            if (mov > 0) parallax1Mov -= mov;

            int right = 0;
            int rightPrev = 0;
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax1].Count; i++)
            {
                scenary.Terrains[TerrainType.Parrallax1][i].X -= mov;
                if (scenary.Terrains[TerrainType.Parrallax1][i].Right < 0)
                {
                    scenary.Terrains[TerrainType.Parrallax1].RemoveAt(i);
                    i--;
                }
                else
                {
                    if (scenary.Terrains[TerrainType.Parrallax1][i].Obsolete)
                    {
                        if (scenary.Terrains[TerrainType.Parrallax1][i].Right > rightPrev)
                            rightPrev = scenary.Terrains[TerrainType.Parrallax1][i].Right;
                    }
                    else
                    {
                        if (scenary.Terrains[TerrainType.Parrallax1][i].Right > right)
                            right = scenary.Terrains[TerrainType.Parrallax1][i].Right;
                    }
                }
            }

            if (right < 640)
            {
                if (parallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax1Y, dunesParallax1Source, dunesParallax1Origin));
                else if (parallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax1Y, ancientRuinsParallax1Source, ancientRuinsParallax1Origin));
            }

            if (parallaxInterpol > 0 && rightPrev < 640)
            {
                if (previousParallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax1Y, dunesParallax1Source, dunesParallax1Origin));
                else if (previousParallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax1].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax1Y, ancientRuinsParallax1Source, ancientRuinsParallax1Origin));
                scenary.Terrains[TerrainType.Parrallax1].Last().Obsolete = true;
            }
        }
        private void UpdateParallax2()
        {
            parallax2Mov += 1;
            int mov = (int)Math.Floor(parallax2Mov);
            if (mov > 0) parallax2Mov -= mov;

            int right = 0;
            int rightPrev = 0;
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax2].Count; i++)
            {
                scenary.Terrains[TerrainType.Parrallax2][i].X -= mov;
                if (scenary.Terrains[TerrainType.Parrallax2][i].Right < 0)
                {
                    scenary.Terrains[TerrainType.Parrallax2].RemoveAt(i);
                    i--;
                }
                else
                {
                    if (scenary.Terrains[TerrainType.Parrallax2][i].Obsolete)
                    {
                        if (scenary.Terrains[TerrainType.Parrallax2][i].Right > rightPrev)
                            rightPrev = scenary.Terrains[TerrainType.Parrallax2][i].Right;
                    }
                    else
                    {
                        if (scenary.Terrains[TerrainType.Parrallax2][i].Right > right)
                            right = scenary.Terrains[TerrainType.Parrallax2][i].Right;
                    }
                }
            }

            if (right < 640)
            {
                if (parallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax2Y, dunesParallax2Source, dunesParallax2Origin));
                else if (parallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax2Y, ancientRuinsParallax2Source, ancientRuinsParallax2Origin));
            }

            if (parallaxInterpol > 0 && rightPrev < 640)
            {
                if (previousParallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax2Y, dunesParallax2Source, dunesParallax2Origin));
                else if (previousParallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax2].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax2Y, ancientRuinsParallax2Source, ancientRuinsParallax2Origin));
                scenary.Terrains[TerrainType.Parrallax2].Last().Obsolete = true;
            }
        }
        private void UpdateParallax3()
        {
            parallax3Mov += 0.5f;
            int mov = (int)Math.Floor(parallax3Mov);
            if (mov > 0) parallax3Mov -= mov;

            int right = 0;
            int rightPrev = 0;
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax3].Count; i++)
            {
                scenary.Terrains[TerrainType.Parrallax3][i].X -= mov;
                if (scenary.Terrains[TerrainType.Parrallax3][i].Right < 0)
                {
                    scenary.Terrains[TerrainType.Parrallax3].RemoveAt(i);
                    i--;
                }
                else
                {
                    if (scenary.Terrains[TerrainType.Parrallax3][i].Obsolete)
                    {
                        if (scenary.Terrains[TerrainType.Parrallax3][i].Right > rightPrev)
                            rightPrev = scenary.Terrains[TerrainType.Parrallax3][i].Right;
                    }
                    else
                    {
                        if (scenary.Terrains[TerrainType.Parrallax3][i].Right > right)
                            right = scenary.Terrains[TerrainType.Parrallax3][i].Right;
                    }
                }
            }

            if (right < 640)
            {
                if (parallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax3Y, dunesParallax3Source, dunesParallax3Origin));
                else if (parallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, parallax3Y, ancientRuinsParallax3Source, ancientRuinsParallax3Origin));
            }

            if (parallaxInterpol > 0 && rightPrev < 640)
            {
                if (previousParallaxBiome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax3Y, dunesParallax3Source, dunesParallax3Origin));
                else if (previousParallaxBiome == BiomeType.AncientRuins)
                    scenary.Terrains[TerrainType.Parrallax3].Add(new Terrain((byte)BiomeType.AncientRuins, desertSpriteSheet, rightPrev, parallax3Y, ancientRuinsParallax3Source, ancientRuinsParallax3Origin));
                scenary.Terrains[TerrainType.Parrallax3].Last().Obsolete = true;
            }
        }

        private void UpdateGround()
        {
            groundMov += 3;
            int mov = (int)Math.Floor(groundMov);
            if (mov > 0) groundMov -= mov;

            int right = 0;
            for (int i = 0; i < scenary.Terrains[TerrainType.Ground].Count; i++)
            {
                scenary.Terrains[TerrainType.Ground][i].X -= mov;
                if (scenary.Terrains[TerrainType.Ground][i].Right < 0)
                {
                    scenary.Terrains[TerrainType.Ground].RemoveAt(i);
                    i--;
                }
                else if (scenary.Terrains[TerrainType.Ground][i].Right > right)
                    right = scenary.Terrains[TerrainType.Ground][i].Right;
            }

            if (right < 640)
            {
                if (biome == BiomeType.Dunes)
                    scenary.Terrains[TerrainType.Ground].Add(new Terrain((byte)BiomeType.Dunes, desertSpriteSheet, right, dunesGroundY, dunesGroundSource, Vector2.Zero));
                else if (biome == BiomeType.AncientRuins)
                {
                    Texture2D t = CreateRuinsFloor(false, false);
                    scenary.Terrains[TerrainType.Ground].Add(new Terrain((byte)BiomeType.AncientRuins, t, right, ancientRuinsGroundY, t.Bounds, Vector2.Zero));
                }
            }
        }
        private Texture2D CreateRuinsFloor(bool start, bool finish) //out Point[] positions
        {
            int finishX = finish ? 1280 - 40 : 1280;
            int startX = start ? 0 : -40;

            RenderTarget2D floor = new RenderTarget2D(General.GraphicsDevice, 1280, 370);//224
            General.GraphicsDevice.SetRenderTarget(floor);
            General.GraphicsDevice.Clear(Color.Transparent);
            General.SpriteBatch.Begin();
            if (start) General.SpriteBatch.Draw(General.TerrainSpriteSheet, ancientCityBack1GroundSource, new Rectangle(0, 464, 192, 224), Color.White); //464
            if (finish) General.SpriteBatch.Draw(General.TerrainSpriteSheet, ancientCityBack3GroundSource, new Rectangle(1280 - 192, 464, 192, 224), Color.White); //464
            for (int y = 380 - 21; y > 370 - 165 - 21; y -= 21)
            {
                int acumX = 0;
                for (int x = startX; x < finishX; x += 40) //*15
                {
                    Rectangle source;
                    int slabIndex = General.Random.Next(0, 20);

                    if (slabIndex < 14) source = new Rectangle(1280 + 64 * 0, 480, 64, 32);
                    else if (slabIndex < 15) source = new Rectangle(1280 + 64 * 1, 480, 64, 32);
                    else if (slabIndex < 16) source = new Rectangle(1280 + 64 * 2, 480, 64, 32);
                    else if (slabIndex < 17) source = new Rectangle(1280 + 64 * 3, 480, 64, 32);
                    else if (slabIndex < 18) source = new Rectangle(1280 + 64 * 4, 480, 64, 32);
                    else source = new Rectangle(1280 + 64 * 5, 480, 64, 32);

                    General.SpriteBatch.Draw(desertSpriteSheet, new Rectangle(x, y, 64, 32), source, Color.White);
                    //acumX += 42;
                }
            }

            for (int x = 0; x < 1280; x += 256)
            {
                if (start && x == 0)
                    General.SpriteBatch.Draw(desertSpriteSheet, new Rectangle(x, 0, 272, 224), ancientCityBack1GroundSource, Color.White);
                else if (finish && x == 1024)
                    General.SpriteBatch.Draw(desertSpriteSheet, new Rectangle(x, 0, 272, 224), ancientCityBack2GroundSource, Color.White);
                else
                    General.SpriteBatch.Draw(desertSpriteSheet, new Rectangle(x, 0, 272, 224), ancientCityBack3GroundSource, Color.White);
            }


            General.SpriteBatch.End();
            General.GraphicsDevice.SetRenderTarget(null);

            //Add traps

            return floor;
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

        protected override void Draw(GameTime gameTime)
        {
            //Render on 640x360 px
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.LightGoldenrodYellow);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            skySprite.Draw();
            foreach (Sprite cs in cloudSprite) cs.Draw();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax3].Count; i++)
                scenary.Terrains[TerrainType.Parrallax3][i].Draw();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax2].Count; i++)
                scenary.Terrains[TerrainType.Parrallax2][i].Draw();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            for (int i = 0; i < scenary.Terrains[TerrainType.Parrallax1].Count; i++)
                scenary.Terrains[TerrainType.Parrallax1][i].Draw();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            for (int i = 0; i < scenary.Terrains[TerrainType.Ground].Count; i++)
                scenary.Terrains[TerrainType.Ground][i].Draw();
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(font, debugInfo, Vector2.Zero, Color.Black);
            spriteBatch.End();
        }
    }

    public enum TerrainType
    {
        Parrallax1,
        Parrallax2,
        Parrallax3,
        Ground,
        Front1,
        Front2,
        Front3,
    }
    public enum BiomeType : byte
    {
        Dunes,
        AncientRuins
    }
    public class Scenary
    {
        public List<Entity> Obstacles = new List<Entity>();
        public Dictionary<TerrainType, List<Terrain>> Terrains = new Dictionary<TerrainType, List<Terrain>>();
    }
    public class Terrain
    {
        public TerrainType TerrainType = TerrainType.Ground;
        public byte BiomeType = 0;

        public bool Obsolete = false;

        public int X 
        {
            get => sprite.X;
            set
            {
                sprite.X = value;
                virtualPosition.X = value;
            }
        }
        public int Y
        {
            get => sprite.Y;
            set
            {
                sprite.Y = value;
                virtualPosition.Y = value;
            }
        }
        public float VX
        {
            get => virtualPosition.X;
            set
            {
                virtualPosition.X = value;
                sprite.X = (int)Math.Floor(value);
            }
        }
        public float VY
        {
            get => virtualPosition.Y;
            set
            {
                virtualPosition.Y = value;
                sprite.Y = (int)Math.Floor(value);
            }
        }

        private Vector2 virtualPosition;
        private Sprite sprite;

        private float speedModificator = 1f;

        public int Right { get => sprite.Right; }
        public float SpeedModificator { get => speedModificator; set => speedModificator = value; }
        public float Depth { get => sprite.Depth; set => sprite.Depth = value; }
        public byte Alpha { get => sprite.Color.A; set => sprite.Color = new Color(sprite.Color, value); }

        public Terrain(byte BiomeType, Texture2D texture, int X, int Y, Rectangle source, Vector2 origin)
        {
            this.BiomeType = BiomeType;
            this.sprite = new Sprite(texture, X, Y, source, Color.White, origin);
            this.Depth = 0;
        }
        public void LoadContent()
        {

        }
        public void Update()
        {
            virtualPosition.X -= General.Speed * speedModificator * General.DeltaTime;
        }
        public void Draw()
        {
            sprite.Draw();
        }
    }
}
