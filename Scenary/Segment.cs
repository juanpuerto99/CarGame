using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarGame.Scenary
{
    public interface EntityData { }

    public class Segment
    {
        public byte BiomeIndex;
        public byte[] TerrainsIndex;
        public EntityData[] Entities;
    }
}
