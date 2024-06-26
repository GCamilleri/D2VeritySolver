using D2VeritySolver.Rooms;
using D2VeritySolver.Shapes;

namespace D2VeritySolver;

public static class EncounterBuilder
{
    public static void RunEncounter()
    {
        bool isValid;
        Callouts callouts = null!;

        var encounter = new Encounter();

        var round = 0;
        do
        {
            round++;
            Console.WriteLine($"Beginning round {round}. Ready for new callouts.");

            Statue? lastDunkedStatue = encounter.LastDunkedStatue ?? null;
            encounter.WithNewCallouts(GetCallouts()).WithLastDunkedStatue(lastDunkedStatue);

            Console.WriteLine();
            Console.WriteLine(encounter.CurrentState());

            new Solver(encounter).SolveSoloRooms();

            Console.WriteLine(encounter.CurrentState());
            Console.WriteLine($"Round {round} complete. Press ENTER to continue");
            Console.ReadLine();
        } while (round < 3);

        Console.WriteLine("Encounter complete!");
        return;

        Callouts GetCallouts()
        {
            do
            {
                string statueCallouts = GetInputUntilValid("Enter the statue shapes from a solo room left-to-right (facing the symbol wall): ");

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

            return callouts;
        }

        string GetInputUntilValid(string prompt)
        {
            bool inputValid;
            var userInput = string.Empty;

            do
            {
                try
                {
                    Console.Write(prompt);

                    userInput = Console.ReadLine();
                    if (userInput is null or "") throw new Exception("Please enter something");

                    inputValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    inputValid = false;
                }
            } while (!inputValid);

            return userInput!;
        }
    }
}