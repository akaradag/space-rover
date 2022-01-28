using System.Collections.Generic;

namespace SpaceRover.Domain
{
    public class Plateau
    {
        public Coordinate MinCoordinate => new Coordinate() { X = 0, Y = 0 };
        public Coordinate MaxCoordinate { get; set; }

        public IList<Rover> Rovers { get; set; }

        public Plateau(Coordinate size)
        {
            MaxCoordinate = size;
            Rovers = new List<Rover>();
        }
    }
}
