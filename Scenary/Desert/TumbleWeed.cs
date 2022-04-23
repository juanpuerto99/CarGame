using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarGame.Scenary.Desert
{
    public class TumbleWeed : Entity
    {
        public bool Direction;
        private TimeSpan rotationDelay;

        public TumbleWeed(Vector2 pos)
        {
            Texture = General.TerrainSpriteSheet;
            Position = pos;
            SourceRectangle = new Rectangle(352, 1072, 48, 48);
            Rectangle = new Rectangle((int)Math.Floor(Position.X), (int)Math.Floor(Position.Y), SourceRectangle.Width, SourceRectangle.Height);

            Origin = new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);

            HitBox = new HitBox((int)Math.Floor(Rectangle.X - Origin.X), (int)Math.Floor(Rectangle.Y - Origin.Y), 11, 32, 28, 11);
            PrevHitBox = HitBox.Clone();
        }
        public override void Update(GameTime gameTime)
        {
            if (IsDisposed) return;

            Position.X -= General.Speed * 1.5f * General.DeltaTime;

            if (Direction) Position.Y -= 1f * General.DeltaTime;
            else Position.Y += 1f * General.DeltaTime;

            rotationDelay += gameTime.ElapsedGameTime;
            if (rotationDelay.TotalMilliseconds > 90)
            {
                rotationDelay = new TimeSpan();
                Rotation -= MathHelper.PiOver4;
            }

            if (Direction)
            {
                if (Position.Y < 160 + 24)
                {
                    Position.Y = 160 + 24;
                    Direction = false;
                }
            }
            else
            {
                if (Position.Y > 360 - 24)
                {
                    Position.Y = 360 - 24;
                    Direction = true;
                }
            }

            UpdateRectanglePosition();
            UpdateHitBox();

            for (int i = 0; i < General.Obstacles.Count; i++)
            {
                if (!(General.Obstacles[i] is Cactus)) continue;

                HitBox.CollisionSide? side = HitBox.CheckCollisionSide(General.Obstacles[i].PrevHitBox, General.Obstacles[i].HitBox);
                if (side != null)
                {
                    if (side == HitBox.CollisionSide.Bottom)
                    {
                        //Ajust...
                        Direction = false;

                        UpdateRectanglePosition();
                        UpdateHitBox();
                    }
                    else if (side == HitBox.CollisionSide.Top)
                    {
                        //Ajust...
                        Direction = true;

                        UpdateRectanglePosition();
                        UpdateHitBox();
                    }
                    else Break();

                    break;
                }
            }

            ApplyDepth();

            if (Rectangle.Right < 0) Dispose();
        }
        public void Break()
        {

        }
    }
}
