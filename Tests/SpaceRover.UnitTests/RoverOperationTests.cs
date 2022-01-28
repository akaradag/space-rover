using SpaceRover.Domain;
using System;
using Xunit;

namespace SpaceRover.UnitTests
{
    public class RoverOperationTests 
    {
        IDomainService domainService;
        public RoverOperationTests()
        {
            this.domainService = new DomainService();
        }

        [Fact]
        public void DoRoverCommand_RoverShouldMoveNorth()
        {
            var expectedRover = new Rover()
            {
                Direction = Directions.North,
                Location = new Coordinate() { X = 1, Y = 2 }
            };

            var plateau = domainService.CreatePlateau(new Coordinate() { X = 5, Y = 5 });
            var actualRover = domainService.DeployRoverToPlateau(new Coordinate() { X = 1, Y = 1 }, Directions.North, null, plateau);
            domainService.DoRoverCommand(Commands.Move, actualRover, plateau);

            Assert.Equal(new object[] { expectedRover.Direction, expectedRover.Location.X, expectedRover.Location.Y }, new object[] { actualRover.Direction, actualRover.Location.X, actualRover.Location.Y });
        }

        [Fact]
        public void DoRoverCommand_RoverShouldTurnLeft()
        {
            var expectedRover = new Rover()
            {
                Direction = Directions.West,
                Location = new Coordinate() { X = 1, Y = 2 }
            };

            var plateau = domainService.CreatePlateau(new Coordinate() { X = 5, Y = 5 });
            var actualRover = domainService.DeployRoverToPlateau(new Coordinate() { X = 1, Y = 2 }, Directions.North, null, plateau);
            domainService.DoRoverCommand(Commands.TurnLeft, actualRover, plateau);

            Assert.Equal(new object[] { expectedRover.Direction, expectedRover.Location.X, expectedRover.Location.Y }, new object[] { actualRover.Direction, actualRover.Location.X, actualRover.Location.Y });
        }

        [Fact]
        public void DoRoverCommand_RoverShouldTurnRight()
        {
            var expectedRover = new Rover()
            {
                Direction = Directions.North,
                Location = new Coordinate() { X = 1, Y = 2 }
            };

            var plateau = domainService.CreatePlateau(new Coordinate() { X = 5, Y = 5 });
            var actualRover = domainService.DeployRoverToPlateau(new Coordinate() { X = 1, Y = 2 }, Directions.West, null, plateau);
            domainService.DoRoverCommand(Commands.TurnRight, actualRover, plateau);

            Assert.Equal(new object[] { expectedRover.Direction, expectedRover.Location.X, expectedRover.Location.Y }, new object[] { actualRover.Direction, actualRover.Location.X, actualRover.Location.Y });
        }

        [Fact]
        public void DoRoverCommand_RoverCannotMoveWestOutOfPlateau()
        {
            var plateau = domainService.CreatePlateau(new Coordinate() { X = 5, Y = 5 });
            var rover = domainService.DeployRoverToPlateau(new Coordinate() { X = 0, Y = 5 }, Directions.West, null, plateau);

            Assert.Throws<Exception>(() => domainService.DoRoverCommand(Commands.Move, rover, plateau));
        }

        [Fact]
        public void DoRoverCommand_RoverCannotBeDeployedOutOfPlateau()
        {
            var plateau = domainService.CreatePlateau(new Coordinate() { X = 5, Y = 5 });

            Assert.Throws<Exception>(() => domainService.DeployRoverToPlateau(new Coordinate() { X = 6, Y = 5 }, Directions.West, null, plateau));
        }

        [Theory]
        [InlineData(new object[] { "5 5", "1 2 N", "LMLMLMLMM", 1, 3, Directions.North })]
        [InlineData(new object[] { "5 5", "3 3 E", "MMRMMRMRRM", 5, 1, Directions.East })]
        public void MarsRoverCase_ShouldGiveExpectedOutputWithTestInputs(string plateauSize, string roverLocation, string roverCommands, int expectedCoordinateX, int expectedCoordinateY, Directions expectedRoverDirection)
        {
            //Parse Plateau Input
            var plateauMaxCoordinateInputs = plateauSize.Split(' ');
            var plateauMaxCoordinate = new Coordinate()
            {
                X = Convert.ToInt32(plateauMaxCoordinateInputs[0]),
                Y = Convert.ToInt32(plateauMaxCoordinateInputs[1])
            };

            //Parse Rover Location Input
            var roverLocationInputs = roverLocation.Split(' ');
            var roverLocationCoordinate = new Coordinate()
            {
                X = Convert.ToInt32(roverLocationInputs[0]),
                Y = Convert.ToInt32(roverLocationInputs[1])
            };
            var roverDirectionInput = EnumHelper.GetValueFromDescription<Directions>(roverLocationInputs[2]);


            //Act
            var plateau = domainService.CreatePlateau(plateauMaxCoordinate);
            var actualRover = domainService.DeployRoverToPlateau(roverLocationCoordinate, roverDirectionInput, null, plateau);
            foreach (var roverCommand in roverCommands)
            {
                var cmd = EnumHelper.GetValueFromDescription<Commands>(roverCommand.ToString());
                domainService.DoRoverCommand(cmd, actualRover, plateau);
            }

            Assert.Equal(expectedCoordinateX, actualRover.Location.X.Value);
            Assert.Equal(expectedCoordinateY, actualRover.Location.Y.Value);
            Assert.Equal(expectedRoverDirection, actualRover.Direction);
        }

    }
}
