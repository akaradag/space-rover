using System.ComponentModel;

namespace SpaceRover.Domain
{
    public enum Commands
    {
        [Description("L")]
        TurnLeft,

        [Description("R")]
        TurnRight,

        [Description("M")]
        Move
    }
}
