using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public class Sprite
    {
        private Texture2D texture;
        private Rectangle rectangle;
        private Rectangle source;
        private Color color;
        private Vector2 origin;
        private float rotation;
        private SpriteEffects spriteEffect;
        private float depth;


        public int X
        {
            get => rectangle.X;
            set => rectangle.X = value;
        }
        public int Y
        {
            get => rectangle.Y;
            set => rectangle.Y = value;
        }
        public int Right
        {
            get => rectangle.Right;
        }
        public int Bottom
        {
            get => rectangle.Bottom;
        }

        public Color Color { get => color; set => color = value; }
        public Vector2 Origin { get => origin; set => origin = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public SpriteEffects SpriteEffect { get => spriteEffect; set => spriteEffect = value; }
        public float Depth { get => depth; set => depth = value; }

        public Sprite(Texture2D texture, int X, int Y, Rectangle source, Color color)
        {
            this.texture = texture;
            this.rectangle = new Rectangle(X, Y, source.Width, source.Height);
            this.source = source;
            this.color = color;
            this.origin = Vector2.Zero;
            this.rotation = 0f;
            this.spriteEffect = SpriteEffects.None;
            this.depth = 1f;
        }
        public Sprite(Texture2D texture, int X, int Y, Rectangle source, Color color, Vector2 origin)
        {
            this.texture = texture;
            this.rectangle = new Rectangle(X, Y, source.Width, source.Height);
            this.source = source;
            this.color = color;
            this.origin = origin;
            this.rotation = 0f;
            this.spriteEffect = SpriteEffects.None;
            this.depth = 1f;
        }

        public void Draw()
        {
            General.SpriteBatch.Draw(texture, rectangle, source, color, rotation, origin, spriteEffect, depth);
        }
    }
}
