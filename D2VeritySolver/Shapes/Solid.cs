using System.Diagnostics;

namespace D2VeritySolver.Shapes;

public class Solid
{
    public List<Shape> Components { get; set; } = [];

    public bool IsValid => Components.Count == 2;

    public static List<Solid> SolvedSolids =
    [
        new Solid { Components = [Shape.Circle, Shape.Triangle] },
        new Solid { Components = [Shape.Circle, Shape.Square] },
        new Solid { Components = [Shape.Square, Shape.Triangle] }
    ];

    public override string ToString()
    {
        if (!IsValid) throw new Exception("Couldn't find a solid shape with those components");

        Shape[] ordered = Components.OrderBy(s => s.AsChar()).ToArray();
        
        if (ordered.SequenceEqual([Shape.Circle, Shape.Triangle])) return "Cone";
        if (ordered.SequenceEqual([Shape.Circle, Shape.Square])) return "Cylinder";
        if (ordered.SequenceEqual([Shape.Circle, Shape.Circle])) return "Sphere";
        if (ordered.SequenceEqual([Shape.Square, Shape.Triangle])) return "Prism";
        if (ordered.SequenceEqual([Shape.Triangle, Shape.Triangle])) return "Pyramid";
        if (ordered.SequenceEqual([Shape.Square, Shape.Square])) return "Cube";

        throw new Exception($"Couldn't find a solid shape with those components: {string.Join(", ", ordered)}");
    }

    public bool IsEquivalentTo(Solid? other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        return other.Components.SequenceEqual(Components);
    }
}