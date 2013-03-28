using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TurnBasedStrategy.Classes.Objects.Terrain
{
    enum Directions { North = 0, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest };
    class Terrain : Object
    {
        public Int32 TerrainHeight;
        public Dictionary<Directions, Boolean> directionPassability;
        public Dictionary<Directions, Terrain> NeighborTerrain;
        public Vector3 position;
        public Biome biome;

        public Terrain()
        {
            directionPassability = new Dictionary<Directions, bool> { };
            NeighborTerrain = new Dictionary<Directions, Terrain> { };

            directionPassability[Directions.North] = true;
            directionPassability[Directions.NorthWest] = true;
            directionPassability[Directions.West] = true;
            directionPassability[Directions.SouthWest] = true;
            directionPassability[Directions.South] = true;
            directionPassability[Directions.SouthEast] = true;
            directionPassability[Directions.East] = true;
            directionPassability[Directions.NorthEast] = true;
            position.X = 0;
            position.Y = 0;
            position.Z = 0;
            biome = new Biome();
        }

        //Constructor with a List of all passable directions from this Terrain tile
        public Terrain(List<Directions> passableDirections)
        {
            directionPassability = new Dictionary<Directions, bool> { };
            NeighborTerrain = new Dictionary<Directions, Terrain> { };
            position.X = 0;
            position.Y = 0;
            position.Z = 0;
            biome = new Biome();

            directionPassability[Directions.North] = false;
            directionPassability[Directions.NorthWest] = false;
            directionPassability[Directions.West] = false;
            directionPassability[Directions.SouthWest] = false;
            directionPassability[Directions.South] = false;
            directionPassability[Directions.SouthEast] = false;
            directionPassability[Directions.East] = false;
            directionPassability[Directions.NorthEast] = false;

            foreach(Directions key in passableDirections){
                directionPassability[key] = true;
            }
        }

        public void AddNeighboringTerrain(Directions directionFromThisTerrain, Terrain terrainToAdd){
            NeighborTerrain.Add(directionFromThisTerrain, terrainToAdd);
        }

        //determines if directionToGo is passable from this tile
        public bool IsDirectionTravelable(Directions directionToGo)
        {
            bool result = false;

            if (NeighborTerrain.ContainsKey(directionToGo) == false)
            {
                return result;
            }

            Terrain neighbor = NeighborTerrain[directionToGo];


            switch (directionToGo)
            {
                case Directions.North:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.South])
                    {
                        result = true;
                    }
                    break;
                case Directions.NorthEast:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.SouthWest])
                    {
                        result = true;
                    }
                    break;
                case Directions.East:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.West])
                    {
                        result = true;
                    }
                    break;
                case Directions.SouthEast:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.NorthWest])
                    {
                        result = true;
                    }
                    break;
                case Directions.South:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.North])
                    {
                        result = true;
                    }
                    break;
                case Directions.SouthWest:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.NorthEast])
                    {
                        result = true;
                    }
                    break;
                case Directions.West:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.East])
                    {
                        result = true;
                    }
                    break;
                case Directions.NorthWest:
                    if (directionPassability[directionToGo] && neighbor.directionPassability[Directions.SouthEast])
                    {
                        result = true;
                    }
                    break;
            }

            return result;
        }
    }

}
