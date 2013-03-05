using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnBasedStrategy.Classes.Objects.Items
{
    class Armor
    {
        private Metal _metal;

        public Armor()
        {
            _metal = new Metal();
        }

        public Armor(Metal mt)
        {
            _metal = mt;
        }

        public static bool operator >(Armor armorA, Armor armorB)
        {
            bool result = false;

            if (armorA._metal > armorB._metal)
            {
                result = true;
            }

            return result;
        }
        public static bool operator <(Armor armorA, Armor armorB)
        {
            bool result = false;

            if (armorA._metal < armorB._metal)
            {
                result = true;
            }

            return result;
        }

        public double GetResist()
        {
            return _metal.GetStrength();
        }
    }
}
