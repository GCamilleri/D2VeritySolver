using D2VeritySolver.Rooms;

namespace D2VeritySolver.Shapes;

public class Statue
{
    public SoloRoom SoloRoom { get; private set; } = null!;
    public Solid Solid { get; set; } = null!;
    public Shape? PrimedShape { get; set; }
    public bool CanPrimeShape(Shape shape) => !IsPrimed && Solid.Components.Contains(shape);
    public bool IsPrimed => PrimedShape != null;

    public bool IsSolved => !Solid.Components.Contains(SoloRoom.StatueShape) && 
                            Solid.SolvedSolids.Any(s => s.IsEquivalentTo(Solid));

    public Statue WithInitialSolid(Solid solid)
    {
        Solid = solid;
        return this;
    }

    public Statue WithAssociatedSoloRoom(SoloRoom soloRoom)
    {
        SoloRoom = soloRoom;
        return this;
    }

    public void PrimeShape(Shape shape)
    {
        if (!CanPrimeShape(shape)) throw new Exception("What the fuck are you trying to do?");
        PrimedShape = shape;
    }

    public void SendPrimedShape(Statue other)
    {
        Solid.Components.Remove(PrimedShape!.Value);
        other.ReceiveShape(PrimedShape!.Value);
        PrimedShape = null;
    }

    public void ReceiveShape(Shape shape)
    {
        Solid.Components.Add(shape);
    }
}