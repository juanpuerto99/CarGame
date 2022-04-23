using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public class DesertStorm
    {
        Texture2D Texture;
        Rectangle zone;
        Rectangle source1;
        Rectangle source2;
        Rectangle source3;
        Vector2 origin1 = Vector2.Zero;
        Vector2 origin2 = Vector2.Zero;
        Vector2 origin3 = Vector2.Zero;

        public DesertStorm(Texture2D texture)
        {
            this.Texture = texture;
            zone = new Rectangle(Point.Zero, Texture.Bounds.Size);
            source1 = source2 = source3 = Texture.Bounds;
        }
        public void Update()
        {
            origin1.X += (0.1f * General.Speed) * General.DeltaTime;
            if (origin1.X > Texture.Width) origin1.X -= Texture.Width;

            origin2.X += (0.15f * General.Speed) * General.DeltaTime;
            origin2.Y += (0.2f * General.Speed) * General.DeltaTime;
            if (origin2.X > Texture.Width) origin2.X -= Texture.Width;
            if (origin2.Y > Texture.Height) origin2.Y -= Texture.Height;

            origin3.X -= (0.3f * General.Speed) * General.DeltaTime;
            origin3.Y -= (0.02f * General.Speed) * General.DeltaTime;

            source1.Location = new Point(Convert.ToInt32(origin1.X), Convert.ToInt32(origin1.Y));
            source2.Location = new Point(Convert.ToInt32(origin2.X), Convert.ToInt32(origin2.Y));
            source3.Location = new Point((int)Math.Floor(origin3.X), (int)Math.Floor(origin3.Y));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, zone, source3, new Color(Color.White, 0.25f));
            spriteBatch.Draw(Texture, zone, source2, new Color(Color.White, 0.5f));
            spriteBatch.Draw(Texture, zone, source1, new Color(Color.White, 0.75f));
        }
    }
}
