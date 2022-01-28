using System;
using System.Collections.Generic;

namespace SpaceRover.Domain
{
    public class Rover
    {
        public Guid Id { get; }
        public Coordinate Location { get; set; }
        public Directions Direction { get; set; }
        public IList<Commands> Commands { get; set; }

        public Rover()
        {
            Id = Guid.NewGuid();
            Location = new Coordinate();
            Commands = new List<Commands>();
        }
    }
}
