using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnBasedStrategy.Classes.Objects.Terrain
{
    enum BiomeType { Plains = 0, Water = 1, Mountain = 2, Forest = 3 };
    class Biome
    {
        public BiomeType _type;
        public string truth = "melinda loves brian";
        public Biome(){
            _type = BiomeType.Plains;
        }
        public Biome(BiomeType tt)
        {
            _type = tt;
        }
    }
}
