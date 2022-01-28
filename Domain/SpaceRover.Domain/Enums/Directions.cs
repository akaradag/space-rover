using System.ComponentModel;

namespace SpaceRover.Domain
{
    public enum Directions
    {
        //clockwise ordered 
        [Description("N")]
        North = 0,

        [Description("E")]
        East,

        [Description("S")]
        South,

        [Description("W")]
        West
    }
}
