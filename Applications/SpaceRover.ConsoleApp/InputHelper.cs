using SpaceRover.Domain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaceRover.ConsoleApp
{
    public static class InputHelper
    {
        public static readonly int expectedRoverCount = 2;
        private static readonly Regex PlateauCoordinatesInputRegex = new Regex(@"^\d+\s\d+$");
        private static readonly Regex RoverCoordinatesInputRegex = new Regex(@"^\d+\s\d+\s[NSWE]$");
        private static readonly Regex RoverCommandsInputRegex = new Regex(@"^[LRM]+$");

        public static RoverInput ValidateAndMapRoverInput(string deployInput, string commandInput)
        {
            if(!RoverCoordinatesInputRegex.Match(deployInput.Trim()).Success)
                throw new Exception("Invalid rover coordinates input");

            if (!RoverCommandsInputRegex.Match(commandInput.Trim()).Success)
                throw new Exception("Invalid rover coordinates input");

            var deployInputs = deployInput.Split(' ');
            var commandList = new List<Commands>();
            foreach (var cmdChar in commandInput)
            {
                commandList.Add(EnumHelper.GetValueFromDescription<Commands>(cmdChar.ToString()));
            }
            return new RoverInput()
            {
                DeployLocation = new Coordinate()
                {
                    X = Convert.ToInt32(deployInputs[0]),
                    Y = Convert.ToInt32(deployInputs[1])
                },
                DeployDirection = EnumHelper.GetValueFromDescription<Directions>(deployInputs[2]),
                Commands = commandList
            };
        }

        public static PlateauInput ValidateAndMapPlateuInput(string input)
        {
            if (!PlateauCoordinatesInputRegex.Match(input.Trim()).Success)
                throw new Exception("Invalid plateau input");

            var inputs = input.Split(' ');
            return new PlateauInput()
            {
                MaxSizes = new Coordinate()
                {
                    X = Convert.ToInt32(inputs[0]),
                    Y = Convert.ToInt32(inputs[1])
                }
            };
        }

    }

    public class PlateauInput
    {
        public Coordinate MaxSizes { get; set; }
    }

    public class RoverInput
    {
        public Coordinate DeployLocation { get; set; }
        public Directions DeployDirection { get; set; }
        public IList<Commands> Commands { get; set; }
    }
}
