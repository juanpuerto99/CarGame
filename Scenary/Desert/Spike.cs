using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Scenary.Desert
{
    public class Spike : Entity
    {
        private int spikeType;

        AnimationSystem animationSystem;

        public Spike(Texture2D texture, int type, Vector2 pos)
        {
            this.Texture = texture;
            spikeType = type;

            AnimationFrame[] frames = new AnimationFrame[12];

            int sourceY = 1440 + (64 * spikeType);
            frames[0] = new AnimationFrame(new Rectangle(0, 1376, 64, 64), 512);
            frames[1] = new AnimationFrame(new Rectangle(64 * 0, sourceY, 64, 64), 32);
            frames[2] = new AnimationFrame(new Rectangle(64 * 1, sourceY, 64, 64), 480);
            frames[3] = new AnimationFrame(new Rectangle(64 * 2, sourceY, 64, 64), 32);
            frames[4] = new AnimationFrame(new Rectangle(64 * 3, sourceY, 64, 64), 16);
            frames[5] = new AnimationFrame(new Rectangle(64 * 4, sourceY, 64, 64), 16);
            frames[6] = new AnimationFrame(new Rectangle(64 * 5, sourceY, 64, 64), 320);
            frames[7] = new AnimationFrame(new Rectangle(64 * 4, sourceY, 64, 64), 32);
            frames[8] = new AnimationFrame(new Rectangle(64 * 3, sourceY, 64, 64), 32);
            frames[9] = new AnimationFrame(new Rectangle(64 * 2, sourceY, 64, 64), 16);
            frames[10] = new AnimationFrame(new Rectangle(64 * 1, sourceY, 64, 64), 16);
            frames[11] = new AnimationFrame(new Rectangle(64 * 0, sourceY, 64, 64), 16);

            AnimationData animationData = new AnimationData("Prick", frames);

            animationSystem = new AnimationSystem();
            animationSystem.SetAnimation(animationData);
            animationSystem.Bucle = true;
            animationSystem.Play();
            SourceRectangle = animationSystem.Source;

            this.Position = pos;
            int xPos = (int)Math.Floor(Position.X);
            int yPos = (int)Math.Floor(Position.Y);
            Rectangle = new Rectangle(xPos, yPos, 64, 64);
            ApplyDepth();

            HitBox = new HitBox(xPos, yPos, 12, 44, 40, 14);
            PrevHitBox = HitBox;
        }
        public override void Update(GameTime gameTime)
        {
            if (IsDisposed) return;

            ApplyScenaryMovement();
            UpdateRectanglePosition();

            animationSystem.Update();
            if (animationSystem.FrameChanged) SourceRectangle = animationSystem.Source;

            if (animationSystem.FrameIndex >= 4 && animationSystem.FrameIndex <= 8) HitBox.Enable = true;
            else HitBox.Enable = false;
            UpdateHitBox();

            if (Rectangle.Right < 0) Dispose();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
    public struct SpikeData : EntityData
    {
        public Point Position;
        public int StartFrame;

        public SpikeData(Point position, int startFrame)
        {
            Position = position;
            StartFrame = startFrame;
        }
    }
}
