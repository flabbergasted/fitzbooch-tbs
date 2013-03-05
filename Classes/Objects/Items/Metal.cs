using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnBasedStrategy.Classes.Objects.Items
{
    enum MetalType {None = 0, Copper, Bronze, Iron, Steel };
    class Metal
    {
        private MetalType _metalType;

        public Metal()
        {
            _metalType = MetalType.None;
        }

        public Metal(MetalType mt)
        {
            _metalType = mt;
        }
        public static bool operator >(Metal metalA, Metal metalB)
        {
            bool result = false;

            if (metalA._metalType > metalB._metalType)
            {
                result = true;
            }

            return result;
        }

        public static bool operator <(Metal metalA, Metal metalB)
        {
            bool result = false;

            if (metalA._metalType < metalB._metalType)
            {
                result = true;
            }

            return result;
        }

        public double GetStrength()
        {
            double result = 0;
            switch (_metalType)
            {
                case MetalType.Copper:
                    result = .1;
                    break;
                case MetalType.Bronze:
                    result = .2;
                    break;
                case MetalType.Iron:
                    result = .3;
                    break;
                case MetalType.Steel:
                    result = .4;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }
    }
}
