using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarGame.Scenary.Desert
{
    public class Cactus : Entity
    {
        public Cactus(Texture2D texture, Vector2 position, int cactusId)
        {
            this.Texture = texture;
            this.Position = position;

            int xPos = (int)Math.Floor(Position.X);
            int yPos = (int)Math.Floor(Position.Y);

            if (cactusId == 0)
            {
                SourceRectangle = new Rectangle(0, 1056, 48, 96);
                HitBox = new HitBox(xPos, yPos, 21, 90, 10, 6);
            }
            else if (cactusId == 1)
            {
                SourceRectangle = new Rectangle(48, 1056, 48, 96);
                HitBox = new HitBox(xPos, yPos, 21, 90, 10, 6);
            }
            else if (cactusId == 2)
            {
                SourceRectangle = new Rectangle(96, 1056, 64, 96);
                HitBox = new HitBox(xPos, yPos, 31, 90, 10, 6);

            }
            else if (cactusId == 3)
            {
                SourceRectangle = new Rectangle(160, 1056, 64, 96);
                HitBox = new HitBox(xPos, yPos, 31, 90, 10, 6);
            }
            else if (cactusId == 4)
            {
                SourceRectangle = new Rectangle(224, 1056, 64, 96);
                HitBox = new HitBox(xPos, yPos, 19, 90, 10, 6);
            }
            else if (cactusId == 5)
            {
                SourceRectangle = new Rectangle(288, 1056, 64, 96);
                HitBox = new HitBox(xPos, yPos, 19, 90, 10, 6);
            }

            Rectangle = new Rectangle(xPos, yPos, SourceRectangle.Width, SourceRectangle.Height);
            Origin = new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height);
            ApplyDepth();

            PrevHitBox = HitBox;
        }
        public override void Update(GameTime gameTime)
        {
            if (IsDisposed) return;
            ApplyScenaryMovement();
            UpdateRectanglePosition();
            UpdateHitBox();

            if (Rectangle.Right < 0) Dispose();
        }
        public void CarCrashed()
        {

        }
        public override void Dispose()
        {
            //Texture.Dispose();
            IsDisposed = true;
        }
    }
}
