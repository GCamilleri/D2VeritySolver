using D2VeritySolver.Shapes;

namespace D2VeritySolver;

public class Callouts
{
    private readonly Dictionary<char, Shape> _stringToShapeMap = new()
    {
        { 'C', Shape.Circle },
        { 'S', Shape.Square },
        { 'T', Shape.Triangle }
    };

    public Shape LeftStatueShape;
    public Shape MiddleStatueShape;
    public Shape RightStatueShape;

    public Solid LeftStatueSolid;
    public Solid MiddleStatueSolid;
    public Solid RightStatueSolid;

    public Dictionary<Shape, List<Shape>> RoomWallCallouts = new();

    public bool TriumphMode;

    public Callouts WithStatueShapes(string statueShapesCallout)
    {
        statueShapesCallout = statueShapesCallout.ToUpper();

        ValidateStatueShapeCallout(statueShapesCallout);

        LeftStatueShape = _stringToShapeMap[statueShapesCallout[0]];
        MiddleStatueShape = _stringToShapeMap[statueShapesCallout[1]];
        RightStatueShape = _stringToShapeMap[statueShapesCallout[2]];

        return this;
    }

    public Callouts WithSoloRoomWallShapes(Shape roomShape, string wallShapes)
    {
        wallShapes = wallShapes.ToUpper();
        
        ValidateRoomWallShapeCallout(wallShapes);

        RoomWallCallouts[roomShape] =
        [
            _stringToShapeMap[wallShapes[0]],
            _stringToShapeMap[wallShapes[1]]
        ];

        return this;
    }

    public Callouts WithStatueSolids(string left, string middle, string right)
    {
        left = left.ToUpper();
        middle = middle.ToUpper();
        right = right.ToUpper();

        ValidateSolidCallout(left);
        ValidateSolidCallout(middle);
        ValidateSolidCallout(right);

        LeftStatueSolid = new Solid { Components = [_stringToShapeMap[left[0]], _stringToShapeMap[left[1]]] };
        MiddleStatueSolid = new Solid { Components = [_stringToShapeMap[middle[0]], _stringToShapeMap[middle[1]]] };
        RightStatueSolid = new Solid { Components = [_stringToShapeMap[right[0]], _stringToShapeMap[right[1]]] };
        
        return this;
    }

    private void ValidateSolidCallout(string solidComponent)
    {
        if (solidComponent.Length != 2) throw new Exception("Two shapes make up a solid");
        IsValidShapeCallout(solidComponent);
    }

    private void ValidateRoomWallShapeCallout(string wallShapeCallout)
    {
        if (wallShapeCallout.Length != 2) throw new Exception("Must be two shapes on the wall per room");
        IsValidShapeCallout(wallShapeCallout);
    }

    private void ValidateStatueShapeCallout(string statueShapesCallout)
    {
        if (statueShapesCallout.Length != 3) throw new Exception("Must be three callouts");
        IsValidShapeCallout(statueShapesCallout);
        if (statueShapesCallout.Distinct().Count() != 3) throw new Exception("Duplicate callouts aren't allowed");
    }

    private void IsValidShapeCallout(string statueShapesCallout)
    {
        if (!statueShapesCallout.All(c => _stringToShapeMap.ContainsKey(c))) throw new Exception("Wtf is that letter bro");
    }

    public Callouts WithTriumphMode(bool triumphMode)
    {
        TriumphMode = triumphMode;
        return this;
    }
}