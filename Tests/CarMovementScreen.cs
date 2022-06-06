using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CarGame.Tests
{
    public class CarMovementScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        RenderTarget2D carRender;
        Texture2D carSpriteSheet;

        private Dictionary<CarAnimationType, AnimationSprite> Animations;
        private CarAnimationType animation = CarAnimationType.Running | CarAnimationType.Shine;
        private int animationFrame;
        private int wheelFrame;
        private bool runBodyMovement;
        private Dictionary<CarPart, PartFrameSprite> PartsSprite;
        private Rectangle[] SpriteSheets;
        public class AnimationSprite
        {
            public FrameSprite[] Frames;
            public bool Loop;
        }
        public class FrameSprite
        {
            public Dictionary<CarPart, FrameDataGeneric> FramexPart;
            public float Duration;
        }
        public class PartFrameSprite
        {
            public Rectangle Rectangle;
            public Rectangle Source;
            public bool Visible;
        }

        //Serial
        public class CarSerialize
        {
            public AnimationData[] Animations;
            [XmlElement(ElementName = "Source")]
            public SerialRectangle[] SpriteSheets;
        }
        public class AnimationData
        {
            public CarAnimationType Animation;
            public FrameData[] Frames;
            public bool Loop;
        }
        public class FrameData
        {
            public FrameDataGeneric[] FramexPart;
            public float Duration;
        }
        [XmlInclude(typeof(PartFrameData)), XmlInclude(typeof(WheelFrameData)), XmlInclude(typeof(MotorFrameData))]
        public abstract class FrameDataGeneric
        {
            public CarPart Part;
            public int X;
            public int Y;
            public bool Visible;
            public bool ApplyCarMovement;
        }
        public class PartFrameData : FrameDataGeneric
        {
            public int Source;
        }
        public class WheelFrameData : FrameDataGeneric
        {
            public int Source1;
            public int Source2;
        }
        public class MotorFrameData : FrameDataGeneric
        {
            private int Source1;
            private int Source2;
            private int Source3;
        }

        public CarMovementScreen() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            this.IsFixedTimeStep = true;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.graphics.PreferredBackBufferWidth = 640 * 3;
            this.graphics.PreferredBackBufferHeight = 360 * 3;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360);

            carSpriteSheet = Content.Load<Texture2D>("Textures/Car");
            carRender = new RenderTarget2D(GraphicsDevice, 144, 80);

            LoadCar();
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            General.GameTime = gameTime;

            KeyboardState ks = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            movementTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
            if (movementTime > 256)
            {
                movementTime = movementTime - 256;
                runBodyMovement = !runBodyMovement;
            }

            float lateralSpeed = 1.5f;
            float lateralSuperSpeed = 3f;
            float frontSpeed = 2f;
            float frontSuperSpeed = 4f;

            Vector2 lastPos = carPosition;
            if (ks.IsKeyDown(Keys.W))
            {
                if (lastLeftpressed > 0)
                { if (lastRightPressed < 32) superLeft = true; }

                if (superLeft) carPosition.Y -= lateralSuperSpeed;
                else carPosition.Y -= lateralSpeed;
                lastLeftpressed = 0;
            }
            else
            {
                lastLeftpressed += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds;
                superLeft = false;
            }

            if (ks.IsKeyDown(Keys.S))
            {
                if (lastRightPressed > 0)
                { if (lastLeftpressed < 32) superRight = true; }

                if (superRight) carPosition.Y += lateralSuperSpeed;
                else carPosition.Y += lateralSpeed;
                lastRightPressed = 0;
            }
            else
            {
                lastRightPressed += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds;
                superRight = false;
            }

            if (ks.IsKeyDown(Keys.D))
            {
                if (lastFrontPressed < 100) superFront = true;

                if (superFront) carPosition.X += frontSuperSpeed;
                else carPosition.X += frontSpeed;
                //lastFrontPressed = 0;
                Dpress = true;
            }
            else
            {
                if (Dpress) lastFrontPressed = 0;

                lastFrontPressed += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds;
                superFront = false;
                Dpress = false;
            }

            if (ks.IsKeyDown(Keys.A))
            {
                if (lastBackPressed < 100) superBack = true;
                if (superBack) carPosition.X -= frontSuperSpeed;
                else carPosition.X -= frontSpeed;
                Apress = true;
            }
            else
            {
                if (Apress) lastBackPressed = 0;

                lastBackPressed += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds;
                superBack = false;
                Apress = false;
            }

            if (lastPos.Y < carPosition.Y)
            {
                if (superRight)
                {
                    if (!animation.HasFlag(CarAnimationType.SuperTurningRight))
                    {
                        if (animation.HasFlag(CarAnimationType.TurningRight))
                            SetAnimation(CarAnimationType.SuperTurningRight, animationFrame);
                        else
                            SetAnimation(CarAnimationType.SuperTurningRight);
                    }
                }
                else
                {
                    if (!animation.HasFlag(CarAnimationType.TurningRight))
                        SetAnimation(CarAnimationType.TurningRight);

                }
            }
            else if (lastPos.Y > carPosition.Y)
            {
                if (superLeft)
                {
                    if (!animation.HasFlag(CarAnimationType.SuperTurningLeft))
                    {
                        if (animation.HasFlag(CarAnimationType.TurningLeft))
                            SetAnimation(CarAnimationType.SuperTurningLeft, animationFrame);
                        else
                            SetAnimation(CarAnimationType.SuperTurningLeft);
                    }
                }
                else
                {
                    if (!animation.HasFlag(CarAnimationType.TurningLeft))
                        SetAnimation(CarAnimationType.TurningLeft);
                }
            }

            if (carPosition.X < lastPos.X)
            {
                //Turning back
                if (superBack)
                {
                    //Turning front
                    if (!animation.HasFlag(CarAnimationType.SuperTurningBack))
                    {
                        if (animation.HasFlag(CarAnimationType.TurningBack))
                            SetAnimation(CarAnimationType.SuperTurningBack, animationFrame);
                        else
                            SetAnimation(CarAnimationType.SuperTurningBack);
                    }
                }
                else
                {
                    if (!animation.HasFlag(CarAnimationType.TurningBack))
                        SetAnimation(CarAnimationType.TurningBack);
                }
            }
            else if (carPosition.X > lastPos.X)
            {
                if (superFront)
                {
                    //Turning front
                    if (!animation.HasFlag(CarAnimationType.SuperTurningFront))
                    {
                        if (animation.HasFlag(CarAnimationType.TurningFront))
                            SetAnimation(CarAnimationType.SuperTurningFront, animationFrame);
                        else
                            SetAnimation(CarAnimationType.SuperTurningFront);
                    }
                }
                else
                {
                    if (!animation.HasFlag(CarAnimationType.TurningFront))
                        SetAnimation(CarAnimationType.TurningFront);
                }
            }

            if (carPosition == lastPos)
            {
                if (!animation.HasFlag(CarAnimationType.Running) && !animation.HasFlag(CarAnimationType.ReturnToRunning))
                {
                    CarAnimationType anim = animation | CarAnimationType.ReturnToRunning;
                    if (Animations.ContainsKey(anim))
                        SetAnimation(anim);
                    else
                        SetAnimation(CarAnimationType.Running);
                }
            }

            carRectangle = new Rectangle((int)Math.Floor(carPosition.X), (int)Math.Floor(carPosition.Y), 144, 80);

            UpdateAnimation();
            UpdateFrames();
        }
        protected override void Draw(GameTime gameTime)
        {
            //Render Car Sprite
            GraphicsDevice.SetRenderTarget(carRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            //Wheel 4
            if (PartsSprite[CarPart.Wheel4].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Wheel4].Rectangle, PartsSprite[CarPart.Wheel4].Source, Color.White);
            //Wheel 3
            if (PartsSprite[CarPart.Wheel3].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Wheel3].Rectangle, PartsSprite[CarPart.Wheel3].Source, Color.White);
            //Draw BodyBack
            if (PartsSprite[CarPart.BodyBack].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.BodyBack].Rectangle, PartsSprite[CarPart.BodyBack].Source, Color.White);
            //Back Seat
            if (PartsSprite[CarPart.BackSeat].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.BackSeat].Rectangle, PartsSprite[CarPart.BackSeat].Source, Color.White);
            //Front Seat 1
            if (PartsSprite[CarPart.FrontSeat1].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.FrontSeat1].Rectangle, PartsSprite[CarPart.FrontSeat1].Source, Color.White);
            //Front Seat 2
            if (PartsSprite[CarPart.FrontSeat2].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.FrontSeat2].Rectangle, PartsSprite[CarPart.FrontSeat2].Source, Color.White);
            //Body Down
            if (PartsSprite[CarPart.BodyDown].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.BodyDown].Rectangle, PartsSprite[CarPart.BodyDown].Source, Color.White);
            //Wheel 2
            if (PartsSprite[CarPart.Wheel2].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Wheel2].Rectangle, PartsSprite[CarPart.Wheel2].Source, Color.White);
            //Wheel 1
            if (PartsSprite[CarPart.Wheel1].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Wheel1].Rectangle, PartsSprite[CarPart.Wheel1].Source, Color.White);
            //Draw Body
            if (PartsSprite[CarPart.Body].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Body].Rectangle, PartsSprite[CarPart.Body].Source, Color.White);
            //Motor / Capo
            if (PartsSprite[CarPart.Motor].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Motor].Rectangle, PartsSprite[CarPart.Motor].Source, Color.White);
            //Door
            if (PartsSprite[CarPart.Door].Visible)
                spriteBatch.Draw(carSpriteSheet, PartsSprite[CarPart.Door].Rectangle, PartsSprite[CarPart.Door].Source, Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            //Render on 640x360 px
            GraphicsDevice.SetRenderTarget(screenRender);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(carRender, carRectangle, Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            //Render on Windows Scale
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();
        }

        Vector2 carPosition = new Vector2();
        Rectangle carRectangle = Rectangle.Empty;

        private bool playing = true;
        private float animationTime = 0f;
        private float wheelTime = 0f;
        private float movementTime = 0f;
        private float speedMod = 1f;
        private bool playingGeneral = true;
        private bool playingWheel = true;

        private bool superLeftEnable = false;
        private bool superRightEnable = false;
        private bool superLeft = false;
        private bool superRight = false;
        private bool superFront = false;
        private bool superBack = false;
        private float lastRightPressed = 10000;
        private float lastLeftpressed = 10000;
        private float lastFrontPressed = 10000;
        private float lastBackPressed = 10000;

        private bool Dpress = false;
        private bool Apress = false;

        private void UpdateAnimation()
        {
            if (!playing) return;

            //Parts
            if (playingGeneral)
            {
                if (animationTime >= Animations[animation].Frames[animationFrame].Duration)
                {
                    animationTime -= Animations[animation].Frames[animationFrame].Duration;
                    animationFrame++;
                    if (animationFrame > Animations[animation].Frames.Length - 1)
                    {
                        if (Animations[animation].Loop) animationFrame = 0;
                        else
                        {
                            animationFrame = Animations[animation].Frames.Length - 1;

                            if (animation.HasFlag(CarAnimationType.ReturnToRunning))
                                SetAnimation(CarAnimationType.Running);
                            else playingGeneral = false;
                        }
                    }
                }
                animationTime += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
            }

            //Wheels
            if (playingWheel)
            {
                if (!superBack)
                {
                    float wheelSpeed = 128;
                    if (superFront) wheelSpeed /= 2;

                    if (wheelTime >= wheelSpeed)
                    {
                        wheelTime = 0;
                        wheelFrame++;
                        if (wheelFrame > 1)
                            wheelFrame = 0;
                    }
                    wheelTime += (float)General.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
                }
            }
        }
        private void UpdateFrames()
        {
            byte[] parts = (byte[])Enum.GetValues(typeof(CarPart));
            for (int i = 0; i < parts.Length; i++)
            {
                CarPart part = (CarPart)parts[i];
                FrameDataGeneric frameData = Animations[animation].Frames[animationFrame].FramexPart[part];

                PartsSprite[part].Visible = frameData.Visible;
                if (frameData.Visible)
                {
                    Rectangle rectangle;
                    Rectangle source;
                    if (frameData.GetType() == typeof(WheelFrameData))
                    {
                        Rectangle source1 = SpriteSheets[((WheelFrameData)frameData).Source1];
                        Rectangle source2 = SpriteSheets[((WheelFrameData)frameData).Source2];
                        source = wheelFrame == 0 ? source1 : source2;

                        rectangle = new Rectangle(frameData.X, frameData.Y, source.Width, source.Height);
                        if (frameData.ApplyCarMovement && runBodyMovement) rectangle.Y += 1;
                    }
                    else
                    {
                        source = SpriteSheets[((PartFrameData)frameData).Source];
                        rectangle = new Rectangle(frameData.X, frameData.Y, source.Width, source.Height);
                        if (frameData.ApplyCarMovement && runBodyMovement) rectangle.Y += 1;

                    }

                    PartsSprite[part].Rectangle = rectangle;
                    PartsSprite[part].Source = source;
                }
            }
        }
        private void SetAnimation(CarAnimationType anim, int frame = 0)
        {
            animationFrame = frame;
            animationTime = 0f;
            animation = anim;
            playingGeneral = true;
        }

        public void LoadCar()
        {
            AnimationSerial[] animationsSerial = null;
            XmlSerializer serializer = new XmlSerializer(typeof(AnimationSerial[]));
            using (var reader = XmlReader.Create("CarSheets.xml"))
            {
                animationsSerial = (AnimationSerial[])serializer.Deserialize(reader);
            }

            CarSerialize carSerialize = null;


            //Conversion
            Rectangle[] r = CreateSpriteSheets();
            SerialRectangle[] newSpriteSheets = new SerialRectangle[r.Length];
            for (int i = 0; i < r.Length; i++)
                newSpriteSheets[i] = new SerialRectangle(r[i]);

            AnimationData[] Ans = new AnimationData[animationsSerial.Length];

            for (int i = 0; i < Ans.Length; i++)
            {
                FrameData[] frameData = new FrameData[animationsSerial[i].Frames.Length];
                for (int j = 0; j < frameData.Length; j++)
                {
                    FrameDataGeneric[] fdg = new FrameDataGeneric[animationsSerial[i].Frames[j].Frame.Length];
                    for (int k = 0; k < fdg.Length; k++)
                    {
                        CarPart part = animationsSerial[i].Frames[j].Frame[k].Part;
                        if (animationsSerial[i].Frames[j].Frame[k].Frame.GetType() == typeof(WheelFrameSerial))
                        {
                            WheelFrameSerial f = (WheelFrameSerial)animationsSerial[i].Frames[j].Frame[k].Frame;
                            int frameIndex1 = Array.FindIndex(r, x => x == f.Source1.GetRectangle());
                            int frameIndex2 = Array.FindIndex(r, x => x == f.Source2.GetRectangle());
                            if (frameIndex1 == -1 || frameIndex2 == -1) 
                            {
                                frameIndex1 = 32;
                                frameIndex2 = 33;
                            }

                            fdg[k] = new WheelFrameData() { X = f.Rectangle.X, Y = f.Rectangle.Y, Source1 = frameIndex1, Source2 = frameIndex2,
                                ApplyCarMovement = f.ApplyCarMovement, Visible = f.Visible, Part = part };  
                        }
                        else if (animationsSerial[i].Frames[j].Frame[k].Frame.GetType() == typeof(PartFrameSerial))
                        {
                            PartFrameSerial f = (PartFrameSerial)animationsSerial[i].Frames[j].Frame[k].Frame;
                            int frameIndex = Array.FindIndex(r, x => x == f.Source.GetRectangle());
                            fdg[k] = new PartFrameData() { X = f.Rectangle.X, Y = f.Rectangle.Y, Source = frameIndex, 
                                ApplyCarMovement = f.ApplyCarMovement, Visible = f.Visible, Part = part };  
                        }
                    }
                    
                    frameData[j] = new FrameData() { Duration = animationsSerial[i].Frames[j].Duration, FramexPart = fdg };
                }

                Ans[i] = new AnimationData() { Loop = animationsSerial[i].Loop, Animation = animationsSerial[i].Animation, Frames = frameData };
            }

            carSerialize = new CarSerialize() { SpriteSheets = newSpriteSheets, Animations = Ans };

            //Serialize
            serializer = new XmlSerializer(typeof(CarSerialize));
            using (var writer = XmlWriter.Create("CarSheets2.xml", new XmlWriterSettings() { Indent = true }))
            {
                serializer.Serialize(writer, carSerialize);
            }

            //Load Sprite Sheets
            Rectangle[] spriteSheets = new Rectangle[carSerialize.SpriteSheets.Length];
            for (int i = 0; i < spriteSheets.Length; i++)
                spriteSheets[i] = carSerialize.SpriteSheets[i].GetRectangle();
            SpriteSheets = spriteSheets;

            //Load Animations
            Animations = new Dictionary<CarAnimationType, AnimationSprite>();
            byte[] parts = (byte[])Enum.GetValues(typeof(CarPart));
            for (int i = 0; i < carSerialize.Animations.Length; i++)
            {
                AnimationData animationData = carSerialize.Animations[i];
                FrameSprite[] frames = new FrameSprite[animationData.Frames.Length];
                for (int j = 0; j < animationData.Frames.Length; j++) 
                {
                    Dictionary<CarPart, FrameDataGeneric> frameXPart = new Dictionary<CarPart, FrameDataGeneric>();

                    for (int k = 0; k < animationData.Frames[j].FramexPart.Length; k++)
                    {
                        FrameDataGeneric frameData = animationData.Frames[j].FramexPart[k];
                        frameXPart.Add(frameData.Part, frameData);
                    }

                    for (int k = 0; k < parts.Length; k++)
                    {
                        CarPart p = (CarPart)parts[k];
                        if (!frameXPart.ContainsKey(p))
                        {
                            if (j == 0)
                            {
                                if (p == CarPart.Wheel1 || p == CarPart.Wheel2 || p == CarPart.Wheel3 || p == CarPart.Wheel4)
                                    frameXPart.Add(p, new WheelFrameData() { Visible = false });
                                else frameXPart.Add(p, new PartFrameData() { Visible = false });
                            }
                            else frameXPart.Add(p, frames[j - 1].FramexPart[p]);
                        }
                    }

                    FrameSprite frame = new FrameSprite() { Duration = animationData.Frames[j].Duration, FramexPart = frameXPart };
                    frames[j] = frame;
                }
                AnimationSprite a = new AnimationSprite() { Frames = frames, Loop = animationData.Loop };
                Animations.Add(animationData.Animation, a);
            }

            PartsSprite = new Dictionary<CarPart, PartFrameSprite>();
            for (int i = 0; i < parts.Length; i++)
            {
                CarPart p = (CarPart)parts[i];
                PartsSprite.Add(p, new PartFrameSprite());
            }
        }
        private Rectangle[] CreateSpriteSheets()
        {
            Dictionary<CarPart, Rectangle[]> SpriteSheetZones = new Dictionary<CarPart, Rectangle[]>();
            int count = 0;

            //Body
            SpriteSheetZones.Add(CarPart.Body, new Rectangle[22]);
            count = 0;
            for (int i = 0; i < 6; i++) SpriteSheetZones[CarPart.Body][count + i] = new Rectangle(144 * i, 64 * 0, 144, 64);
            count += 6;
            for (int i = 0; i < 4; i++) SpriteSheetZones[CarPart.Body][count + i] = new Rectangle(144 * i, 64 * 1, 144, 64);
            count += 4;
            for (int i = 0; i < 4; i++) SpriteSheetZones[CarPart.Body][count + i] = new Rectangle(144 * i, 64 * 2, 144, 64);
            count += 4;
            for (int i = 0; i < 4; i++) SpriteSheetZones[CarPart.Body][count + i] = new Rectangle(144 * i, 64 * 3, 144, 64);
            count += 4;
            for (int i = 0; i < 4; i++) SpriteSheetZones[CarPart.Body][count + i] = new Rectangle(144 * i, 64 * 4, 144, 64);
            count += 4;

            //Body Down
            SpriteSheetZones.Add(CarPart.BodyDown, new Rectangle[3]);
            count = 0;
            for (int i = 0; i < 3; i++) SpriteSheetZones[CarPart.BodyDown][count + i] = new Rectangle(144 * i, 320, 144, 32);
            count += 3;

            //Body Back
            SpriteSheetZones.Add(CarPart.BodyBack, new Rectangle[9]);
            count = 0;
            for (int i = 0; i < 9; i++) SpriteSheetZones[CarPart.BodyBack][count + i] = new Rectangle(96 * i, 352, 96, 48);
            count += 9;

            //BackSeat
            SpriteSheetZones.Add(CarPart.BackSeat, new Rectangle[5]);
            count = 0;
            for (int i = 0; i < 5; i++) SpriteSheetZones[CarPart.BackSeat][count + i] = new Rectangle(32 * i, 400, 32, 32);
            count += 5;

            //Front Seats
            SpriteSheetZones.Add(CarPart.FrontSeat1, new Rectangle[3]);
            SpriteSheetZones.Add(CarPart.FrontSeat2, new Rectangle[3]);
            count = 0;
            for (int i = 0; i < 3; i++)
            {
                SpriteSheetZones[CarPart.FrontSeat1][count + i] = new Rectangle(160 + 32 * i, 400, 32, 32);
                SpriteSheetZones[CarPart.FrontSeat2][count + i] = SpriteSheetZones[CarPart.FrontSeat1][count + i];
            }
            count += 3;

            //Wheels
            SpriteSheetZones.Add(CarPart.Wheel1, new Rectangle[8]);
            SpriteSheetZones.Add(CarPart.Wheel2, new Rectangle[8]);
            SpriteSheetZones.Add(CarPart.Wheel3, new Rectangle[8]);
            SpriteSheetZones.Add(CarPart.Wheel4, new Rectangle[8]);
            count = 0;
            for (int i = 0; i < 8; i++)
            {
                SpriteSheetZones[CarPart.Wheel1][count + i] = new Rectangle(32 * i, 432, 32, 32);
                SpriteSheetZones[CarPart.Wheel2][count + i] = SpriteSheetZones[CarPart.Wheel1][count + i];
                SpriteSheetZones[CarPart.Wheel3][count + i] = SpriteSheetZones[CarPart.Wheel1][count + i];
                SpriteSheetZones[CarPart.Wheel4][count + i] = SpriteSheetZones[CarPart.Wheel1][count + i];
            }
            count += 8;

            //Door
            SpriteSheetZones.Add(CarPart.Door, new Rectangle[7]);
            count = 0;
            for (int i = 0; i < 7; i++) SpriteSheetZones[CarPart.Door][count + i] = new Rectangle(48 * i, 464, 48, 48);
            count += 7;

            //Capo / Motor
            SpriteSheetZones.Add(CarPart.Motor, new Rectangle[16]);
            count = 0;
            for (int i = 0; i < 6; i++) SpriteSheetZones[CarPart.Motor][count + i] = new Rectangle(464 + 48 * i, 400, 48, 32);
            count += 6;
            for (int i = 0; i < 3; i++) SpriteSheetZones[CarPart.Motor][count + i] = new Rectangle(464 + 48 * i, 432, 48, 32);
            count += 3;
            for (int i = 0; i < 3; i++) SpriteSheetZones[CarPart.Motor][count + i] = new Rectangle(464 + 48 * i, 464, 48, 32);
            count += 3;
            for (int i = 0; i < 2; i++) SpriteSheetZones[CarPart.Motor][count + i] = new Rectangle(464 + 48 * i, 464 + 32, 48, 32);
            count += 2;
            for (int i = 0; i < 2; i++) SpriteSheetZones[CarPart.Motor][count + i] = new Rectangle(464 + 48 * i, 464 + 64, 48, 32);
            count += 2;


            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (CarPart part in SpriteSheetZones.Keys)
            {
                if (part == CarPart.Wheel2 || part == CarPart.Wheel3 || part == CarPart.Wheel4 || part == CarPart.FrontSeat2) continue;
                rectangles.AddRange(SpriteSheetZones[part]);
            }

            return rectangles.ToArray();
        }
    }
}
