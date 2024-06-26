using System.Diagnostics;
using D2VeritySolver.Rooms;
using D2VeritySolver.Shapes;

namespace D2VeritySolver;

public class Solver(Encounter encounter)
{
    private Encounter Encounter { get; } = encounter;
    
    public void SolveSoloRooms()
    {
        var executedCommands = new List<string>();

        while (!Encounter.ShapeStatueMap.Values.All(s => s.SoloRoom.IsSolved))
        {
            IEnumerable<string> commands = Encounter.GenerateAllAvailableSoloRoomCommands();
            var commandScores = new Dictionary<string, int>();

            foreach (string command in commands)
            {
                ExecuteCommand(command);
                int score = Encounter.CalculateSoloRoomsScore();
                ExecuteCommand(CommandUtils.InvertCommand(command), isUndo: true);

                commandScores.Add(command, score);
            }

            string commandToExecute = commandScores
                .Where(cs => !(executedCommands.Count(c => c == cs.Key) >= 2))
                .Where(cs => !Encounter.TriumphMode || Encounter.ShapeStatueMap[cs.Key[3].AsShape()] != Encounter.LastDunkedStatue)
                .MaxBy(c => c.Value).Key;

            ExecuteCommand(commandToExecute);
            Encounter.LastDunkedStatue = Encounter.ShapeStatueMap[commandToExecute[3].AsShape()];
            executedCommands.Add(commandToExecute);

            //PrintUpdate(commandToExecute);
        }

        Console.WriteLine();
        Console.WriteLine(string.Join(Environment.NewLine, executedCommands.Select(CommandUtils.PrintCommand)));
        Console.WriteLine();
    }

    private void ExecuteCommand(string command, bool isUndo = false)
    {
        if (command == string.Empty) return;

        Debug.Assert(command.Length == 4);

        if (command[0] == 'X')
        {
            Shape room = command[1].AsShape();
            Shape shape = command[2].AsShape();
            Shape target = command[3].AsShape();

            Encounter.ShapeStatueMap[room].SoloRoom.SendShape(shape, Encounter.ShapeStatueMap[target].SoloRoom, isUndo: isUndo);
        }
        else
        {
            Shape firstStatue = command[0].AsShape();
            Shape firstShape = command[1].AsShape();
            Shape secondStatue = command[2].AsShape();
            Shape secondShape = command[3].AsShape();

            Encounter.ShapeStatueMap[firstStatue].PrimeShape(firstShape);
            Encounter.ShapeStatueMap[secondStatue].PrimeShape(secondShape);
        }
    }
    
    private void PrintUpdate(string command)
    {
        Console.Clear();

        Console.WriteLine(CommandUtils.PrintCommand(command));

        Console.WriteLine("Passes performed");
        Console.WriteLine($"{string.Join(" | ", Encounter.Statues
            .Select(s => $"{s.SoloRoom.StatueShape.AsChar()} Room: {s.SoloRoom.PassesPerformed}"))}");

        Console.WriteLine($"Score = {Encounter.CalculateSoloRoomsScore()}");
        Debug.Assert(Encounter.LastDunkedStatue != null, "Encounter.LastDunkedStatue != null");
        Console.WriteLine($"Last Dunked = {Encounter.LastDunkedStatue.SoloRoom.StatueShape}");

        Console.WriteLine(Encounter.CurrentState());

        Console.ReadLine();
    }
}