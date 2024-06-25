// See https://aka.ms/new-console-template for more information

using D2VeritySolver;
using D2VeritySolver.Rooms;
using D2VeritySolver.Shapes;

Console.WriteLine("Hello, Guardian!");

bool isValid;
Callouts callouts = null!;

do
{
    string statueCallouts = GetInputUntilValid("Enter the statue shapes from a solo room left-to-right: ");

    string leftStatueSolid = GetInputUntilValid("Enter the components for the solid shape on the left statue (eg: Cone = ct): ");
    string middleStatueSolid = GetInputUntilValid("Enter the components for the solid shape on the middle statue (eg: Cone = ct): ");
    string rightStatueSolid = GetInputUntilValid("Enter the components for the solid shape on the right statue (eg: Cone = ct): ");

    string triangleRoomWallShapes = GetInputUntilValid("Enter the shapes on the wall in the room the triangle player is in: ");
    string circleRoomWallShapes = GetInputUntilValid("Enter the shapes on the wall in the room the circle player is in: ");
    string squareRoomWallShapes = GetInputUntilValid("Enter the shapes on the wall in the room the square player is in: ");
    string triumphMode = GetInputUntilValid("Triumph mode? Y/N:");
    
    try
    {
        callouts = new Callouts()
            .WithStatueShapes(statueCallouts)
            .WithStatueSolids(leftStatueSolid, middleStatueSolid, rightStatueSolid)
            .WithSoloRoomWallShapes(Shape.Triangle, triangleRoomWallShapes)
            .WithSoloRoomWallShapes(Shape.Circle, circleRoomWallShapes)
            .WithSoloRoomWallShapes(Shape.Square, squareRoomWallShapes)
            .WithTriumphMode(triumphMode == "Y");

        isValid = true;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);

        isValid = false;
    }
} while (!isValid);

var encounter = new Encounter(callouts);

Console.WriteLine();
Console.WriteLine(encounter.CurrentState());

new Solver(encounter).SolveSoloRooms();

Console.WriteLine(encounter.CurrentState());

return;

string GetInputUntilValid(string prompt)
{
    bool inputValid;
    var retval = string.Empty;

    do
    {
        try
        {
            Console.Write(prompt);

            retval = Console.ReadLine();
            if (retval is null or "") throw new Exception("Please enter something");

            inputValid = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            inputValid = false;
        }
    } while (!inputValid);

    return retval!;
}