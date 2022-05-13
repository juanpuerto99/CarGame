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

            Terrains.Add(Terrain.eTerrainType.Front2, new List<Terrain>());
            Texture2D t2 = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 688, 1280, 48));
            Terrains[Terrain.eTerrainType.Front2].Add(new Terrain(t2, Terrain.eTerrainType.Front2, 0, 360 - 32));

            Terrains.Add(Terrain.eTerrainType.Front3, new List<Terrain>());

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

            LoadSegments();

            nextTumbleWeedDelay = TimeSpan.FromSeconds(random.Next(5, 15));
            maxDistanceOnBiome = 1;
        }
        public void LoadSegments()
        {
            //random.Next(214 + 20, 360)
            Segment s1 = new Segment()
            {
                Entities = new EntityData[3]
                {
                    new CactusData(new Point(100, 240)),
                    new CactusData(new Point(200, 240)),
                    new TumbleWeedData(new Point(300, 300), true)
                },
                TerrainsIndex = new byte[1] { 0 },
                BiomeIndex = (byte)BiomeType.Desert
            };
            Segment s2 = new Segment()
            {
                Entities = new EntityData[1]
                {
                    new SpikeData(new Point(0, 0), 0)
                },
                TerrainsIndex = new byte[1] { 0 },
                BiomeIndex = (byte)BiomeType.AncientRuins
            };

            Segments.Add(BiomeType.Desert, new List<Segment>());
            Segments.Add(BiomeType.AncientRuins, new List<Segment>());

            Segments[BiomeType.Desert].Add(s1);
            Segments[BiomeType.AncientRuins].Add(s2);
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

            CheckNextBiome(right);

            if (right < 640)
            {
                if (biome == BiomeType.Desert)
                {
                    List<Segment> seg = Segments[BiomeType.Desert].Where(x => Array.Exists(x.TerrainsIndex, y => y == 0)).ToList();
                    Segment s = seg[random.Next(0, seg.Count)];

                    for (int i = 0; i < s.Entities.Length; i++)
                    {
                        Type type = s.Entities[i].GetType();
                        if (type == typeof(CactusData))
                        {
                            CactusData cactusData = (CactusData)s.Entities[i];
                            int textureId = General.Random.Next(0, 6);
                            Vector2 pos = new Vector2(right + cactusData.Position.X, cactusData.Position.Y);
                            Cactus cactus = new Cactus(General.TerrainSpriteSheet, pos, textureId);
                            Obstacles.Add(cactus);
                        }
                        else if (type == typeof(TumbleWeedData))
                        {
                            TumbleWeedData tumbleWeedData = (TumbleWeedData)s.Entities[i];
                            Vector2 pos = new Vector2(right + tumbleWeedData.Position.X, tumbleWeedData.Position.Y);
                            TumbleWeed tumbleWeed = new TumbleWeed(pos);
                            tumbleWeed.Direction = Convert.ToBoolean(random.Next(0, 1));
                            Obstacles.Add(tumbleWeed);
                        }
                    }

                    Color[] c = new Color[1280 * 224];
                    General.TerrainSpriteSheet.GetData(0, new Rectangle(0, 464, 1280, 224), c, 0, c.Length);
                    Texture2D t = new Texture2D(General.GraphicsDevice, 1280, 224);
                    t.SetData(c);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Ground, right, 360 - 224);
                    tn.Speed = General.Speed;
                    Terrains[Terrain.eTerrainType.Ground].Add(tn);
                }
                else if (biome == BiomeType.AncientRuins)
                {
                    Texture2D t = CreateRuinsFloor(distanceOnBiome == 0, false);
                    Terrain tn = new Terrain(t, Terrain.eTerrainType.Ground, right, 360 - 224 - 160);
                    tn.Speed = General.Speed;
                    Terrains[Terrain.eTerrainType.Ground].Add(tn);

                    int spikeQ = random.Next(4, 12);
                    for (int i = 0; i < spikeQ; i++)
                    {

                    }

                    Spike spike1 = new Spike(General.TerrainSpriteSheet, random.Next(0, 3), new Vector2(right, 280));
                    Spike spike2 = new Spike(General.TerrainSpriteSheet, random.Next(0, 3), new Vector2(right, 220));
                    Spike spike3 = new Spike(General.TerrainSpriteSheet, random.Next(0, 3), new Vector2(right, 160));
                    Obstacles.Add(spike1);
                    Obstacles.Add(spike2);
                    Obstacles.Add(spike3);
                }
            }

            //Front 1
            //right = -1;
            //for (int i = 0; i < Terrains[Terrain.eTerrainType.Front1].Count; i++)
            //{
            //    Terrains[Terrain.eTerrainType.Front1][i].Update();
            //    if (!Terrains[Terrain.eTerrainType.Front1][i].isDisposed)
            //        right = Math.Max(right, Terrains[Terrain.eTerrainType.Front1][i].virtualX + Terrains[Terrain.eTerrainType.Front1][i].Zone.Width);
            //    else
            //    {
            //        Terrains[Terrain.eTerrainType.Front1].RemoveAt(i);
            //        i--;
            //    }
            //}
            //if (right < 640)
            //{
            //    if (right == -1) right = 640;
            //    if (biome == BiomeType.AncientRuins)
            //    {
            //        Texture2D wall = CreateRuinFront(distanceOnBiome == 0, false);
            //        Terrain ftn = new Terrain(wall, Terrain.eTerrainType.Front1, right, 360 - 200);
            //        ftn.Speed = 5 * 1.15f;
            //        Terrains[Terrain.eTerrainType.Front1].Add(ftn);
            //    }
            //}

            //Front 2
            right = -1;
            for (int i = 0; i < Terrains[Terrain.eTerrainType.Front2].Count; i++)
            {
                Terrains[Terrain.eTerrainType.Front2][i].Update();
                if (!Terrains[Terrain.eTerrainType.Front2][i].isDisposed)
                    right = Math.Max(right, Terrains[Terrain.eTerrainType.Front2][i].virtualX + Terrains[Terrain.eTerrainType.Front2][i].Zone.Width);
                else
                {
                    Terrains[Terrain.eTerrainType.Front2].RemoveAt(i);
                    i--;
                }
            }

            if (right < 640)
            {
                if (biome == BiomeType.AncientRuins)
                {
                    if (parralaxCurrentBiome == BiomeType.Desert)
                    {
                        Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(368, 1152, 256, 224));
                        Terrain tn = new Terrain(t, Terrain.eTerrainType.Front2, right, 360 - 184);
                        tn.Speed = General.Speed * 1.3f;
                        Terrains[Terrain.eTerrainType.Front2].Add(tn);
                    }
                    else
                    {
                        Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(624, 1152, 256, 224));
                        Terrain tn = new Terrain(t, Terrain.eTerrainType.Front2, right, 360 - 184);
                        tn.Speed = General.Speed * 1.3f;
                        Terrains[Terrain.eTerrainType.Front2].Add(tn);
                    }
                    parralaxCurrentBiome = BiomeType.AncientRuins;
                }
                else
                {
                    if (parralaxCurrentBiome == BiomeType.AncientRuins)
                    {
                        Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(880, 1152, 256, 224));
                        Terrain tn = new Terrain(t, Terrain.eTerrainType.Front2, right, 360 - 184);
                        tn.Speed = General.Speed * 1.3f;
                        Terrains[Terrain.eTerrainType.Front2].Add(tn);
                    }
                    else
                    {
                        //Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 688, 1280, 48));
                        Texture2D t = General.TerrainSpriteSheet.GetTexture(new Rectangle(0, 688, 256, 48));
                        Terrain tn = new Terrain(t, Terrain.eTerrainType.Front2, right, 360 - 32);
                        tn.Speed = General.Speed * 1.3f;
                        Terrains[Terrain.eTerrainType.Front2].Add(tn);
                    }
                    parralaxCurrentBiome = BiomeType.Desert;
                }
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

            if (biome == BiomeType.Desert)
            {
                //CactusUpdate(gameTime);
                //TubleWeedUpdate(gameTime);
            }

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
            distanceOnBiome += General.Speed;
        }

        private void CheckNextBiome(float right)
        {
            if (right < 640)
            {
                if (distanceOnBiome > maxDistanceOnBiome)
                {
                    //Change biome
                    distanceOnBiome = 0;

                    List<byte> posibleNewBiome = new List<byte>();
                    switch (biome)
                    {
                        case BiomeType.Desert:
                            posibleNewBiome.Add((byte)BiomeType.AncientRuins);
                            break;
                        case BiomeType.AncientRuins:
                            posibleNewBiome.Add((byte)BiomeType.Desert);
                            break;
                    }

                    int newBiomeIndex = random.Next(0, posibleNewBiome.Count);
                    biome = (BiomeType)posibleNewBiome[newBiomeIndex];

                    switch (biome)
                    {
                        case BiomeType.Desert:
                            maxDistanceOnBiome = 5000;
                            break;
                        case BiomeType.AncientRuins:
                            maxDistanceOnBiome = 3000;
                            break;
                    }
                }
            }
        }

        Dictionary<BiomeType, List<Segment>> Segments = new Dictionary<BiomeType, List<Segment>>();
        private enum BiomeType : byte
        {
            Desert = 0,
            AncientRuins = 1
        }
        private BiomeType biome;
        private BiomeType parralaxCurrentBiome;
        private float distanceOnBiome;
        private int maxDistanceOnBiome;
        private void CactusUpdate(GameTime gameTime)
        {
            cactusDelay += gameTime.ElapsedGameTime;

            if (cactusDelay.TotalSeconds > 2)
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
            }
        }
        private void TubleWeedUpdate(GameTime gameTime)
        {
            tumbleWeedDelay += gameTime.ElapsedGameTime;

            if (tumbleWeedDelay > nextTumbleWeedDelay)
            {
                tumbleWeedDelay = new TimeSpan();
                nextTumbleWeedDelay = TimeSpan.FromSeconds(random.Next(2, 5));

                Vector2 pos = new Vector2(640 + 50, random.Next(214 + 20, 350));
                TumbleWeed tumbleWeed = new TumbleWeed(pos);
                tumbleWeed.Direction = Convert.ToBoolean(random.Next(0, 1));
                Obstacles.Add(tumbleWeed);
            }
        }

        int[,] slabs;
        private Texture2D CreateRuinsFloor(bool start, bool finish) //out Point[] positions
        {
            int finishX = finish ? 1280 - 5 : 1280 - 5 + 43;

            RenderTarget2D floor = new RenderTarget2D(General.GraphicsDevice, 1280, 370);//224
            General.GraphicsDevice.SetRenderTarget(floor);
            General.GraphicsDevice.Clear(Color.Transparent);
            General.SpriteBatch.Begin();
            if (start) General.SpriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(0, 120, 192, 224), new Rectangle(0, 424, 192, 224), Color.White); //464
            for (int y = 380 - 21; y > 370 - 165 - 21; y -= 21)
            {
                int acumX = 0;
                for (int x = 5; x < finishX; x += 42) //*15
                {
                    Rectangle source;
                    int slabIndex = random.Next(0, 10);

                    if (slabIndex < 6) source = new Rectangle(64, 1376, 64, 32);
                    else if (slabIndex < 7) source = new Rectangle(128, 1376, 64, 32);
                    else if (slabIndex < 8) source = new Rectangle(192, 1376, 64, 32);
                    else if (slabIndex < 9) source = new Rectangle(64, 1408, 64, 32);
                    else if (slabIndex < 10) source = new Rectangle(128, 1408, 64, 32);
                    else source = new Rectangle(192, 1408, 64, 32);

                    General.SpriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(x, y, 64, 32), source, Color.White);
                    //acumX += 42;
                }
            }

            for (int x = start ? 0 : -352 / 2; x < 1280; x += 256)
                General.SpriteBatch.Draw(General.TerrainSpriteSheet, new Rectangle(x, 0, 352, 224), new Rectangle(0, 1152, 352, 224), Color.White);


            General.SpriteBatch.End();
            General.GraphicsDevice.SetRenderTarget(null);

            //Add traps

            return floor;
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
