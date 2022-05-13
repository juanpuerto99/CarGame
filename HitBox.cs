using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public class HitBox
    {
        private int paddingX;
        private int paddingY;
        private int width;
        private int height;

        public int PaddingX
        {
            get => paddingX;
            set
            {
                paddingX = value;
                rectangle.X = x + value;
            }
        }
        public int PaddingY
        {
            get => paddingY;
            set
            {
                paddingY = value;
                rectangle.Y = y + value;
            }
        }
        public int Width
        {
            get => width;
            set
            {
                width = value;
                rectangle.Width = width + value;
            }
        }
        public int Height
        {
            get => height;
            set
            {
                height = value;
                rectangle.Height = height + value;
            }
        }

        private bool enable = true;
        public bool Enable { get => enable; set => enable = value; }

        private int x;
        public int X
        {
            get => x;
            set
            {
                x = value;
                rectangle.X = value + PaddingX;
            }
        }

        private int y;
        public int Y
        {
            get => y;
            set
            {
                y = value;
                rectangle.Y = value + PaddingY;
            }
        }

        private Rectangle rectangle;
        public Rectangle Rectangle { get => rectangle; }

        public HitBox()
        {
            paddingX = paddingY = 0;
            width = height = 100;
            rectangle = new Rectangle(x + paddingX, y + paddingY, width, height);
        }
        public HitBox(int X, int Y, int paddingX, int paddingY, int width, int height)
        {
            this.paddingX = paddingX;
            this.paddingY = paddingY;
            this.width = width;
            this.height = height;

            rectangle = new Rectangle(x + paddingX, y + paddingY, width, height);
        }

        public bool CheckCollision(HitBox hitbox)
        {
            if (!enable || !hitbox.enable) return false;
            return rectangle.Contains(hitbox.rectangle);
        }
        public bool CheckCollision(HitBox previousHitBox, HitBox currentHitBox)
        {
            if (!enable || !currentHitBox.enable) return false;
            return false;
            //if (CheckCollision(currentHitBox))
            //{
            //    int leftD = rectangle.Left - previousHitBox.rectangle.Left;
            //    int right = previousHitBox.Left - 
            //}
            //else return false;
        }
        public CollisionSide? CheckCollisionSide(HitBox previousHitBox, HitBox currentHitBox)
        {
            if (!enable || !currentHitBox.enable) return null;
            if (!rectangle.Contains(currentHitBox.rectangle)) return null;

            if (previousHitBox.Rectangle.Right < rectangle.Left) return CollisionSide.Left;
            else if (previousHitBox.Rectangle.Left > rectangle.Right) return CollisionSide.Right;
            else if (previousHitBox.Rectangle.Bottom < rectangle.Top) return CollisionSide.Top;
            else return CollisionSide.Bottom;
        }

        public HitBox Clone()
        {
            return (HitBox)this.MemberwiseClone();
        }

        public enum CollisionSide
        {
            Top,
            Bottom,
            Right,
            Left
        }
    }
}
