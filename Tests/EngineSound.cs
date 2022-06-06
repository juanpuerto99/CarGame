using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Tests
{
    public class EngineSound : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D screenRender;

        private bool motorOn = false;
        private float speed = 0;

        SpriteFont font;
        string debugInfo;

        public EngineSound()
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
            screenRender = new RenderTarget2D(GraphicsDevice, 640, 360);

            font = Content.Load<SpriteFont>("Fonts/Arial12");

            ignitionSound = Content.Load<SoundEffect>("Sounds/Motor/Ignition");
            rpmSound1 = Content.Load<SoundEffect>("Sounds/Motor/RPM-1070");
            rpmSound2 = Content.Load<SoundEffect>("Sounds/Motor/RPM-1529");
            rpmSound3 = Content.Load<SoundEffect>("Sounds/Motor/RPM-2328");
            base.LoadContent();
        }
        private SoundEffect ignitionSound;
        private SoundEffect rpmSound1;
        private SoundEffect rpmSound2;
        private SoundEffect rpmSound3;

        private SoundEffectInstance rpmSoundInstance1;
        private SoundEffectInstance rpmSoundInstance2;
        private SoundEffectInstance rpmSoundInstance3;
        private SoundEffectInstance ignitionSoundInstance;

        RPMSoundInstance rpmSoundSelected = RPMSoundInstance.None;
        public enum RPMSoundInstance
        {
            None,
            RpmSound1,
            RpmSound2,
            RpmSound3
        }

        struct SpeedInstance
        {
            public int Min;
            public int Max;

            public SpeedInstance(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }

        SpeedInstance startSpeed2 = new SpeedInstance(50, 100);
        SpeedInstance startSpeed3 = new SpeedInstance(200, 300);

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            General.GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            float newSpeed = speed;
            float frictionaForce = 0.5f;
            if (motorOn)
            {
                if (ks.IsKeyDown(Keys.W))
                {
                    if (ks.IsKeyDown(Keys.LeftShift)) newSpeed += 3 * General.DeltaTime;
                    else newSpeed += frictionaForce + 0.25f * General.DeltaTime;
                }
                if (ks.IsKeyDown(Keys.S))
                {
                    if (ks.IsKeyDown(Keys.LeftShift)) newSpeed -= 3 * General.DeltaTime;
                    else newSpeed -= 2 - frictionaForce * General.DeltaTime;
                }
            }

            if (newSpeed > 0) newSpeed -= frictionaForce * General.DeltaTime;
            if (newSpeed < 0) newSpeed = 0;

            if (ks.IsKeyDown(Keys.O))
            {
                if (!motorOn)
                {
                    motorOn = true;
                    ignitionSoundInstance = ignitionSound.CreateInstance();
                    ignitionSoundInstance.Play();
                }
            }
            if (ks.IsKeyDown(Keys.P))
            {
                if (motorOn)
                {
                    motorOn = false;
                }
            }

            speed = newSpeed;
            if (speed == 0)
            {
                if (rpmSoundSelected != RPMSoundInstance.None)
                {
                    rpmSoundSelected = RPMSoundInstance.None;
                    if (rpmSoundInstance1 != null && rpmSoundInstance1.State == SoundState.Playing) rpmSoundInstance1.Stop();
                }
            }

            if (motorOn && (ignitionSoundInstance == null || ignitionSoundInstance.State != SoundState.Playing))
            {
                //Sound 1
                if (speed < startSpeed2.Max)
                {
                    if (rpmSoundInstance1 == null || rpmSoundInstance1.State != SoundState.Playing)
                    {
                        rpmSoundInstance1 = rpmSound1.CreateInstance();
                        rpmSoundInstance1.IsLooped = true;
                        rpmSoundInstance1.Play();
                    }

                    if (speed < startSpeed2.Min) rpmSoundInstance1.Volume = 1f;
                    else
                    {
                        float v = (startSpeed2.Max - speed) / (startSpeed2.Max - startSpeed2.Min);
                        rpmSoundInstance1.Volume = v;
                    }
                    float p = 1 - (startSpeed2.Max - speed) /  startSpeed2.Max;
                    rpmSoundInstance1.Pitch = p;
                }
                else
                {
                    if (rpmSoundInstance1 != null && rpmSoundInstance1.State == SoundState.Playing) rpmSoundInstance1.Stop();
                }

                //Sound 2
                if (speed > startSpeed2.Min && speed < startSpeed3.Max)
                {
                    if (rpmSoundSelected != RPMSoundInstance.RpmSound2)
                    {
                        rpmSoundSelected = RPMSoundInstance.RpmSound2;

                    }
                    if (rpmSoundInstance2 == null || rpmSoundInstance2.State != SoundState.Playing)
                    {
                        rpmSoundInstance2 = rpmSound2.CreateInstance();
                        rpmSoundInstance2.IsLooped = true;
                        rpmSoundInstance2.Play();
                    }

                    if (speed < startSpeed2.Max)
                    {
                        float v = 1 - (startSpeed2.Max - speed) / (startSpeed2.Max - startSpeed2.Min);
                        rpmSoundInstance2.Volume = v;
                    }
                    else if (speed < startSpeed3.Min)
                        rpmSoundInstance2.Volume = 1f;
                    else
                    {
                        float v = (startSpeed3.Max - speed) / (startSpeed3.Max - startSpeed3.Min);
                        rpmSoundInstance2.Volume = v;
                    }

                    float p = 1 - (startSpeed3.Max - speed) / (startSpeed3.Max - startSpeed2.Min);
                    rpmSoundInstance2.Pitch = p / 2;
                }
                else
                {
                    if (rpmSoundInstance2 != null && rpmSoundInstance2.State == SoundState.Playing) rpmSoundInstance2.Stop();
                }
            
                //Sound 3
                if (speed > startSpeed3.Min)
                {
                    if (rpmSoundInstance3 == null || rpmSoundInstance3.State != SoundState.Playing)
                    {
                        rpmSoundInstance3 = rpmSound3.CreateInstance();
                        rpmSoundInstance3.IsLooped = true;
                        rpmSoundInstance3.Play();
                    }

                    if (speed < startSpeed3.Max)
                    {
                        float v = 1 - ((startSpeed3.Max - speed) / (startSpeed3.Max - startSpeed3.Min));
                        rpmSoundInstance3.Volume = v;
                    }
                    else
                        rpmSoundInstance3.Volume = 1f;

                    float maxSpeed = startSpeed3.Max + 100;
                    float p = 1 - (maxSpeed - speed) / (maxSpeed - startSpeed3.Min);
                    rpmSoundInstance3.Pitch = Math.Min(1, p);
                }
                else
                {
                    if (rpmSoundInstance3 != null && rpmSoundInstance3.State == SoundState.Playing) rpmSoundInstance3.Stop();
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ignition: " + motorOn);
            sb.AppendLine("Speed: " + (int)speed);

            string s;
            sb.Append("RPM S1: ");
            if (rpmSoundInstance1 != null && rpmSoundInstance1.State == SoundState.Playing)
                sb.Append(rpmSoundInstance1.Volume.ToString("0.00") + " | " + rpmSoundInstance1.Pitch.ToString("0.00"));
            else sb.Append("-");

            sb.AppendLine();
            sb.Append("RPM S2: ");
            if (rpmSoundInstance2 != null && rpmSoundInstance2.State == SoundState.Playing)
                sb.Append(rpmSoundInstance2.Volume.ToString("0.00") + " | " + rpmSoundInstance2.Pitch.ToString("0.00"));
            else sb.Append("-");

            sb.AppendLine();
            sb.Append("RPM S3: ");
            if (rpmSoundInstance3 != null && rpmSoundInstance3.State == SoundState.Playing)
                sb.Append(rpmSoundInstance3.Volume.ToString("0.00") + " | " + rpmSoundInstance3.Pitch.ToString("0.00"));
            else sb.Append("-");
            debugInfo = sb.ToString();
        }
        protected override void Draw(GameTime gameTime)
        {
            //Render on 640x360 px
            //GraphicsDevice.SetRenderTarget(screenRender);
            //GraphicsDevice.Clear(Color.Transparent);
            //spriteBatch.Begin();
            //spriteBatch.Draw()
            //spriteBatch.End();
            //GraphicsDevice.SetRenderTarget(null);

            //Render on Windows Scale
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(screenRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(font, debugInfo, Vector2.Zero, Color.Black);
            spriteBatch.End();
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
