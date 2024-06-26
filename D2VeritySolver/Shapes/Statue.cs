using D2VeritySolver.Rooms;

namespace D2VeritySolver.Shapes;

public class Statue
{
    public SoloRoom SoloRoom { get; private set; } = null!;
    public Solid CurrentSolid { get; set; } = null!;
    public Shape? PrimedShape { get; set; }
    public bool CanPrimeShape(Shape shape) => !IsPrimed && CurrentSolid.Components.Contains(shape);
    public bool IsPrimed => PrimedShape != null;

    public bool IsSolved => !CurrentSolid.Components.Contains(SoloRoom.StatueShape) && 
                            Solid.SolvedSolids.Any(s => s.IsEquivalentTo(CurrentSolid));

    public Statue WithInitialSolid(Solid solid)
    {
        CurrentSolid = solid;
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
        CurrentSolid.Components.Remove(PrimedShape!.Value);
        other.ReceiveShape(PrimedShape!.Value);
        PrimedShape = null;
    }

    public void ReceiveShape(Shape shape)
    {
        CurrentSolid.Components.Add(shape);
    }
}