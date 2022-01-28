using System.Collections.Generic;

namespace SpaceRover.Domain
{
    public interface IDomainService
    {
        Plateau CreatePlateau(Coordinate size);
        Rover DeployRoverToPlateau(Coordinate coordinate, Directions direction, IList<Commands> roverCommands, Plateau plateau);
        Rover CreateRover(Coordinate coordinate, Directions direction, IList<Commands> commands);
        void DoRoverCommand(Commands command, Rover rover, Plateau plateau);
        void ExecuteRoverCommands(Rover rover, Plateau plateau);
    }
}
