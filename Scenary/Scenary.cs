using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Scenary
{
    public abstract class Scenary
    {
        public Dictionary<Terrain.eTerrainType, List<Terrain>> Terrains = new Dictionary<Terrain.eTerrainType, List<Terrain>>();
        public List<Entity> Obstacles = new List<Entity>();

        public virtual void LoadContent(ContentManager Content) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void PostDraw(GameTime gameTime) { }
    }
}
