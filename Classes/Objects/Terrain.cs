using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnBasedStrategy
{
    enum Directions { North = 0, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest };
    class Terrain : Object
    {
        public Int32 TerrainHeight;
        public bool NorthPassable;
        public bool NorthWestPassable;
        public bool WestPassable;
        public bool SouthWestPassable;
        public bool SouthPassable;
        public bool SouthEastPassable;
        public bool EastPassable;
        public bool NorthEastPassable;
        public Dictionary<Directions, Terrain> NeighborTerrain;

        //TODO:Add constructors

        //TODO:determines if directionToGo is passable from this tile
        public bool IsDirectionTravelable(Directions directionToGo)
        {
            switch (directionToGo)
            {
                case Directions.North:
                    break;
                case Directions.NorthEast:
                    break;
                case Directions.East:
                    break;
                case Directions.SouthEast:
                    break;
                case Directions.South:
                    break;
                case Directions.SouthWest:
                    break;
                case Directions.West:
                    break;
                case Directions.NorthWest:
                    break;
            }


        }
    }

}
