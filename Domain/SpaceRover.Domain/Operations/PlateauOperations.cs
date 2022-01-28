using System;
using System.Collections.Generic;

namespace SpaceRover.Domain
{
    public partial class DomainService : IDomainService
    {
        public Plateau CreatePlateau(Coordinate size)
        {
            return new Plateau(size);
        }

        public Rover DeployRoverToPlateau(Coordinate coordinate, Directions direction, IList<Commands> roverCommands, Plateau plateau)
        {
            foreach (var existingRover in plateau.Rovers)
            {
                if (existingRover.Location.X == coordinate.X && existingRover.Location.Y == coordinate.Y)
                    throw new Exception($"Can't deploy rover to ({coordinate.X},{coordinate.Y})! There is another rover on there!");
            }

            var rover = CreateRover(coordinate, direction, roverCommands);

            if (rover.Location.X > plateau.MaxCoordinate.X || rover.Location.X < plateau.MinCoordinate.X
                || rover.Location.Y > plateau.MaxCoordinate.Y || rover.Location.Y < plateau.MinCoordinate.Y)
                throw new Exception($"Can't deploy rover! Plateau doesn't have this coordinates ({rover.Location.X},{rover.Location.Y})!");

            plateau.Rovers.Add(rover);
            return rover;
        }
    }
}
