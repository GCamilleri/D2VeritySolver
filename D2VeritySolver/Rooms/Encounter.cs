using System.Diagnostics;
using System.Text;
using D2VeritySolver.Shapes;

namespace D2VeritySolver.Rooms;

public class Encounter
{
    public Statue? LastDunkedStatue { get; set; }

    public bool TriumphMode { get; set; }

    public Encounter WithLastDunkedStatue(Statue? statue)
    {
        LastDunkedStatue = statue;
        return this;
    }
    
    public void SetLastDunked(string statue)
    {
        LastDunkedStatue = statue switch
        {
            "L" => LeftStatue,
            "M" => MiddleStatue,
            "R" => RightStatue,
            "X" => null ,
            _ => throw new Exception("Yo that is not L, M, R")
        };
    }
    
    public Encounter WithNewCallouts(Callouts callouts)
    {
        LeftStatue = new Statue()
            .WithInitialSolid(callouts.LeftStatueSolid)
            .WithAssociatedSoloRoom(new SoloRoom(callouts.LeftStatueShape)
                .WithWallShapes(callouts.RoomWallCallouts[callouts.LeftStatueShape]));
        MiddleStatue = new Statue()
            .WithInitialSolid(callouts.MiddleStatueSolid)
            .WithAssociatedSoloRoom(new SoloRoom(callouts.MiddleStatueShape)
                .WithWallShapes(callouts.RoomWallCallouts[callouts.MiddleStatueShape]));
        RightStatue = new Statue()
            .WithInitialSolid(callouts.RightStatueSolid)
            .WithAssociatedSoloRoom(new SoloRoom(callouts.RightStatueShape)
                .WithWallShapes(callouts.RoomWallCallouts[callouts.RightStatueShape]));

        ShapeStatueMap = new Dictionary<Shape, Statue>
        {
            { LeftStatue.SoloRoom.StatueShape, LeftStatue },
            { MiddleStatue.SoloRoom.StatueShape, MiddleStatue },
            { RightStatue.SoloRoom.StatueShape, RightStatue }
        };

        TriumphMode = callouts.TriumphMode;

        SetLastDunked(callouts.LastDunked);
        return this;
    }

    public Dictionary<Shape, Statue> ShapeStatueMap { get; private set; }

    private Statue LeftStatue { get; set; }

    private Statue MiddleStatue { get; set; }

    private Statue RightStatue { get; set; }

    public Statue[] Statues =>
    [
        LeftStatue, MiddleStatue, RightStatue
    ];

    public int StatuesPrimed => new[]
    {
        LeftStatue, MiddleStatue, RightStatue
    }.Count(p => p.IsPrimed);

    public void PrimeLeft(Shape shape)
    {
        PrimeStatue(LeftStatue, shape);
    }

    public void PrimeMiddle(Shape shape)
    {
        PrimeStatue(MiddleStatue, shape);
    }

    public void PrimeRight(Shape shape)
    {
        PrimeStatue(RightStatue, shape);
    }

    private void PrimeStatue(Statue statue, Shape shape)
    {
        Debug.Assert(StatuesPrimed < 2);

        statue.PrimeShape(shape);

        if (StatuesPrimed == 2) SwapShapes();
    }

    private void SwapShapes()
    {
        Statue[] primedStatues = Statues.Where(p => p.IsPrimed).ToArray();

        Debug.Assert(primedStatues.Length == 2);
        Debug.Assert(primedStatues[0].PrimedShape.HasValue);
        Debug.Assert(primedStatues[1].PrimedShape.HasValue);

        primedStatues[0].SendPrimedShape(primedStatues[1]);
        primedStatues[1].SendPrimedShape(primedStatues[0]);

        Debug.Assert(Statues.All(s => s.CurrentSolid.IsValid));
    }

    public string CurrentState()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Left statue:");
        sb.AppendLine($"    Solo room:");
        sb.AppendLine($"      - Statue shape: {LeftStatue.SoloRoom.StatueShape}");
        sb.AppendLine($"      - Wall shapes: {string.Join(", ", LeftStatue.SoloRoom.WallShapes)}");
        sb.AppendLine($"      - Shapes received: {string.Join(", ", LeftStatue.SoloRoom.ShapesReceived)}");
        sb.AppendLine($"      - Passes completed: {LeftStatue.SoloRoom.PassesPerformed}");
        sb.AppendLine($"      - Solved: {LeftStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine($"    Main Room:");
        sb.AppendLine($"    - Current: {LeftStatue.CurrentSolid}");
        sb.AppendLine($"    - Target: {LeftStatue.SoloRoom.TargetSolid}");
        sb.AppendLine($"    - Solved: {LeftStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine();


        sb.AppendLine($"Middle statue:");
        sb.AppendLine($"    Solo room:");
        sb.AppendLine($"      - Statue shape: {MiddleStatue.SoloRoom.StatueShape}");
        sb.AppendLine($"      - Wall shapes: {string.Join(", ", MiddleStatue.SoloRoom.WallShapes)}");
        sb.AppendLine($"      - Shapes received: {string.Join(", ", MiddleStatue.SoloRoom.ShapesReceived)}");
        sb.AppendLine($"      - Passes completed: {MiddleStatue.SoloRoom.PassesPerformed}");
        sb.AppendLine($"      - Solved: {MiddleStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine($"    Main Room:");
        sb.AppendLine($"    - Current: {MiddleStatue.CurrentSolid}");
        sb.AppendLine($"    - Target: {MiddleStatue.SoloRoom.TargetSolid}");
        sb.AppendLine($"    - Solved: {MiddleStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine();

        sb.AppendLine($"Right statue:");
        sb.AppendLine($"    Solo room:");
        sb.AppendLine($"      - Statue shape: {RightStatue.SoloRoom.StatueShape}");
        sb.AppendLine($"      - Wall shapes: {string.Join(", ", RightStatue.SoloRoom.WallShapes)}");
        sb.AppendLine($"      - Shapes received: {string.Join(", ", RightStatue.SoloRoom.ShapesReceived)}");
        sb.AppendLine($"      - Passes completed: {RightStatue.SoloRoom.PassesPerformed}");
        sb.AppendLine($"      - Solved: {RightStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine($"    Main Room:");
        sb.AppendLine($"    - Current: {RightStatue.CurrentSolid}");
        sb.AppendLine($"    - Target: {RightStatue.SoloRoom.TargetSolid}");
        sb.AppendLine($"    - Solved: {RightStatue.SoloRoom.IsSolved.ToString().ToUpper()}");
        sb.AppendLine();

        return sb.ToString();
    }

    public int CalculateSoloRoomsScore() =>
        Statues.Sum(s => s.SoloRoom.ShadowsCleansed * 300) +
        Statues.Sum(s => s.SoloRoom.IsSolved ? 10000 : 0) +
        (int)(1 / (Statues.Average(s => s.SoloRoom.PassesPerformed - 2) + 1)) * 100 +
        Statues.Count(s => s.SoloRoom.WallShapes.Count == 2) * 500;

    public IEnumerable<string> GenerateAllAvailableSoloRoomCommands()
    {
        var commands = new List<string>();

        foreach (SoloRoom soloRoom in Statues.Select(s => s.SoloRoom))
        {
            if (soloRoom.IsSolved && !TriumphMode) continue;

            commands.AddRange(
                (from wallshape in soloRoom.WallShapes
                    from target in soloRoom.TargetSolid.Components.Where(s => !TriumphMode || !ShapeStatueMap[s].SoloRoom.IsSolved)
                    select new[] { wallshape, target })
                .Where(x => !TriumphMode || x[0] != x[1])
                .Select(x => $"X{soloRoom.StatueShape.AsChar()}{x[0].AsChar()}{x[1].AsChar()}"));
        }

        return commands.Distinct();
    }
}