using D2VeritySolver.Shapes;

namespace D2VeritySolver;

public static class CommandUtils
{
    public static Shape AsShape(this char input)
    {
        return input switch
        {
            'T' => Shape.Triangle,
            'C' => Shape.Circle,
            'S' => Shape.Square,
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };
    }

    public static char AsChar(this Shape input)
    {
        return input switch
        {
            Shape.Triangle => 'T',
            Shape.Circle => 'C',
            Shape.Square => 'S',
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };
    }

    public static Shape GetRandomShapeChar(Shape? ignore = null)
    {
        var rand = new Random();
        Shape retval;

        do
        {
            int random = rand.Next(100);

            retval = random switch
            {
                >= 0 and <= 33 => Shape.Triangle,
                >= 33 and <= 66 => Shape.Circle,
                >= 66 and <= 100 => Shape.Square,
                _ => throw new Exception("It's ass, brother")
            };
        } while (retval == ignore);

        return retval;
    }

    public static Shape PickShape(IEnumerable<Shape> shapes)
    {
        var rand = new Random();

        IEnumerable<Shape> shapeArray = shapes.ToArray();
        return shapeArray.ToArray()[rand.Next(shapeArray.Count() - 1)];
    }

    public static string InvertCommand(string command)
    {
        if (command[0] == 'X')
        {
            char room = command[3];
            char shape = command[2];
            char target = command[1];

            return $"X{room}{shape}{target}";
        }
        else throw new NotImplementedException();
    }

    public static string PrintCommand(string command)
    {
        if (command[0] == 'X')
        {
            Shape room = command[1].AsShape();
            Shape shape = command[2].AsShape();
            Shape target = command[3].AsShape();

            return $"{room} pass {shape} to {target}";
        }
        else throw new NotImplementedException();
    }
}