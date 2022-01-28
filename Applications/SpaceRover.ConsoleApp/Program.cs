using SpaceRover.Domain;
using System;

namespace SpaceRover.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            IDomainService domainService = new DomainService();
            bool errorFlag;

            do
            {
                errorFlag = false;
                Console.WriteLine("\nINPUT: \n");
                PlateauInput plateuInput;
                Plateau plateau;

                try
                {
                    plateuInput = InputHelper.ValidateAndMapPlateuInput(Console.ReadLine());
                    plateau = domainService.CreatePlateau(plateuInput.MaxSizes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    errorFlag = true;
                    Console.WriteLine("\nPress ESC to close the console or any key to reenter inputs.. \n");
                    continue;
                }

                for (int i = 0; i < InputHelper.expectedRoverCount; i++)
                {
                    try
                    {
                        var roverInput = InputHelper.ValidateAndMapRoverInput(Console.ReadLine(), Console.ReadLine());
                        domainService.DeployRoverToPlateau(roverInput.DeployLocation, roverInput.DeployDirection, roverInput.Commands, plateau);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        errorFlag = true;
                        break;
                    }


                    if (i == InputHelper.expectedRoverCount - 1)
                    {
                        Console.WriteLine("\nAdd another rover? (Y/N) \n");

                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            i--;
                        }
                        else
                            break;
                    }
                }

                if (errorFlag)
                {
                    Console.WriteLine("\nPress ESC to close the console or any key to reenter inputs.. \n");
                    continue;
                }
                Console.WriteLine("OUTPUT:\n");

                foreach (var rover in plateau.Rovers)
                {
                    try
                    {
                        domainService.ExecuteRoverCommands(rover, plateau);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        errorFlag = true;
                        Console.WriteLine("\nPress ESC to close the console or any key to reenter inputs.. \n");
                        break;
                    }
                    Console.WriteLine($"{rover.Location.X} {rover.Location.Y} {EnumHelper.GetEnumDescription(rover.Direction)}");
                }

                Console.WriteLine("\nPress ESC to close the console or any key to reenter inputs.. \n");

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
