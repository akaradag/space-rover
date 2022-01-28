using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceRover.Domain
{
    public partial class DomainService : IDomainService
    {
        public Rover CreateRover(Coordinate coordinate, Directions direction, IList<Commands> commands)
        {
            return new Rover()
            {
                Location = coordinate,
                Direction = direction,
                Commands = commands
            };
        }

        public void DoRoverCommand(Commands command, Rover rover, Plateau plateau)
        {
            var directionValues = Enum.GetValues(typeof(Directions)).Cast<Directions>();

            switch (command)
            {
                case Commands.TurnLeft:
                    rover.Direction = rover.Direction - 1 < directionValues.Min() ? directionValues.Max() : rover.Direction - 1;
                    break;
                case Commands.TurnRight:
                    rover.Direction = rover.Direction + 1 > directionValues.Max() ? directionValues.Min() : rover.Direction + 1;
                    break;
                case Commands.Move:
                    MoveRoverForward(rover, plateau);
                    break;
                default:
                    throw new Exception("Can't move! Unimplemented command!");
            }
        }

        public void ExecuteRoverCommands(Rover rover, Plateau plateau)
        {
            foreach (var command in rover.Commands)
            {
                DoRoverCommand(command, rover, plateau);
            }
        }

        private void MoveRoverForward(Rover rover, Plateau plateau)
        {
            switch (rover.Direction)
            {
                case Directions.North:
                    ValidateRoverMovement(new Coordinate() { X = rover.Location.X, Y = rover.Location.Y + 1 }, rover, plateau);
                    rover.Location.Y++;
                    break;
                case Directions.South:
                    ValidateRoverMovement(new Coordinate() { X = rover.Location.X, Y = rover.Location.Y - 1 }, rover, plateau);
                    rover.Location.Y--;
                    break;
                case Directions.West:
                    ValidateRoverMovement(new Coordinate() { X = rover.Location.X - 1, Y = rover.Location.Y }, rover, plateau);
                    rover.Location.X--;
                    break;
                case Directions.East:
                    ValidateRoverMovement(new Coordinate() { X = rover.Location.X + 1, Y = rover.Location.Y }, rover, plateau);
                    rover.Location.X++;
                    break;
                default:
                    throw new Exception("Can't move! Unimplemented direction!");
            }
        }

        private void ValidateRoverMovement(Coordinate desiredCoordinate, Rover rover, Plateau plateau)
        {
            if (!rover.Location.X.HasValue || !rover.Location.Y.HasValue || plateau.Rovers.Where(x => x.Id == rover.Id).FirstOrDefault() == null)
                throw new Exception("Can't move! Deploy the rover first");

            if (desiredCoordinate.X < plateau.MinCoordinate.X || desiredCoordinate.X > plateau.MaxCoordinate.X
                || desiredCoordinate.Y < plateau.MinCoordinate.Y || desiredCoordinate.Y > plateau.MaxCoordinate.Y)
                throw new Exception($"Can't move to the {rover.Direction}! Plateau ends at coordinates: ({rover.Location.X},{rover.Location.Y})");

            foreach (var existingRover in plateau.Rovers)
            {
                if (existingRover.Id == rover.Id)
                    continue;

                if (existingRover.Location.X == desiredCoordinate.X && existingRover.Location.Y == desiredCoordinate.Y)
                    throw new Exception($"Can't move from ({rover.Location.X},{rover.Location.Y}) to the {rover.Direction}! Another rover blocks the way at: ({existingRover.Location.X},{existingRover.Location.Y})");
            }
        }
    }
}
