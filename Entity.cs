using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public abstract class Entity
    {
        public Texture2D Texture;
        public Vector2 Position = Vector2.Zero;
        public Rectangle Rectangle;
        public Rectangle SourceRectangle;
        public Vector2 Origin = Vector2.Zero;
        public float Rotation = 0f;
        public float Depth = 1f;

        public HitBox HitBox;
        public HitBox PrevHitBox;
        public bool IsDisposed = false;

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime)
        {
            if (IsDisposed) return;
            General.SpriteBatch.Draw(Texture, Rectangle, SourceRectangle, Color.White, Rotation, Origin, SpriteEffects.None, Depth);
        }
        public virtual void DrawHitBox(GameTime gameTime)
        {
            if (PrevHitBox != null)
                General.SpriteBatch.Draw(General.Pixel, PrevHitBox.Rectangle, new Color(Color.Orange, 0.2f));

            if (HitBox != null)
                General.SpriteBatch.Draw(General.Pixel, HitBox.Rectangle, new Color(Color.Red, 0.4f));
        }

        public virtual void Dispose() { IsDisposed = true; }

        public void ApplyScenaryMovement() { Position.X -= General.Speed * General.DeltaTime; }
        public void UpdateRectanglePosition()
        {
            Rectangle.X = (int)Math.Floor(Position.X);
            Rectangle.Y = (int)Math.Floor(Position.Y);
        }
        public void ApplyDepth()
        {
            float d = (Rectangle.Bottom - Origin.Y) / 640f;
            Depth = Math.Min(1, Math.Max(0, d));
            Console.WriteLine(Depth);
        }
        public void UpdateHitBox()
        {
            PrevHitBox = HitBox.Clone();
            HitBox.X = (int)Math.Floor(Rectangle.X - Origin.X);
            HitBox.Y = (int)Math.Floor(Rectangle.Y - Origin.Y);
        }
    }
}
