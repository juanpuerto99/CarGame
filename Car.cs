using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame
{
    public class Car : Entity
    {
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < General.Obstacles.Count; i++)
            {
                if (HitBox.CheckCollision(General.Obstacles[i].HitBox))
                {
                    //General.Obstacles[i].CarCrashed();
                }
            }
        }
    }
}
