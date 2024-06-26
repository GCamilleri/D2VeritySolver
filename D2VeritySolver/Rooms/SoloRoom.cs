using D2VeritySolver.Shapes;

namespace D2VeritySolver.Rooms;

public class SoloRoom
{
    public SoloRoom(Shape shape)
    {
        StatueShape = shape;
        TargetSolid = Solid.SolvedSolids.Single(x => !x.Components.Contains(shape));
    }

    public Shape StatueShape { get; }
    public Solid TargetSolid { get; }
    public List<Shape> WallShapes { get; private set; } = [];
    private Stack<Shape> ReceivedShapeStack { get; } = new();
    public IEnumerable<Shape> ShapesReceived => ReceivedShapeStack.Distinct();

    public int ShadowsCleansed => ShapesReceived.Distinct().Intersect(TargetSolid.Components).Count();

    public int PassesPerformed { get; private set; }

    public bool IsSolved => WallShapes.Count == 2 &&
                            ShadowsCleansed == 2 &&
                            !WallShapes.Except(TargetSolid.Components).Any() &&
                            PassesPerformed >= 2;

    private bool CanSend(Shape shape) => WallShapes.Contains(shape);
    
    public SoloRoom WithWallShapes(List<Shape> wallShapes)
    {
        WallShapes = wallShapes;
        return this;
    }

    public void SendShape(Shape shape, SoloRoom targetRoom, bool isUndo = false)
    {
        if (!CanSend(shape)) throw new Exception("What the fuck are you trying to do?");
        WallShapes.Remove(shape);

        if (!isUndo)
            PassesPerformed++;
        else
            ReceivedShapeStack.Pop();

        targetRoom.ReceiveShape(shape, isUndo: isUndo);
    }

    private void ReceiveShape(Shape shape, bool isUndo = false)
    {
        WallShapes.Add(shape);
        switch (isUndo)
        {
            case true:
                PassesPerformed--;
                break;
            case false:
                ReceivedShapeStack.Push(shape);
                break;
        }
    }

    public string GenerateRandomCommand()
    {
        if (WallShapes.Count == 0) return string.Empty;

        char room = StatueShape.AsChar();
        char shape = CommandUtils.PickShape(WallShapes).AsChar();
        char target = CommandUtils.GetRandomShapeChar(ignore: StatueShape).AsChar();

        return $"X{room}{shape}{target}";
    }
}