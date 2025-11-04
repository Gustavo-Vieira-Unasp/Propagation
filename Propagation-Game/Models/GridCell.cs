using Raylib_cs;
using System.Numerics;
using Interfaces.Grid;
namespace Models.Grid;

public class TriangleCell : IGridCell
{
    public Vector2 A { get; }
    public Vector2 B { get; }
    public Vector2 C { get; }
    public bool IsBuilt { get; set; } = false;

    public static readonly Color DefaultColor = new Color(60, 100, 160, 255);
    public static readonly Color BuiltColor = new Color(200, 160, 60, 255);

    public Color CurrentColor => IsBuilt ? BuiltColor : DefaultColor;

    public TriangleCell(Vector2 a, Vector2 b, Vector2 c)
    {
        A = a; B = b; C = c;
    }

    public bool Contains(Vector2 p)
    {
        var v0 = C - A;
        var v1 = B - A;
        var v2 = p - A;

        var dot00 = Vector2.Dot(v0, v0);
        var dot01 = Vector2.Dot(v0, v1);
        var dot02 = Vector2.Dot(v0, v2);
        var dot11 = Vector2.Dot(v1, v1);
        var dot12 = Vector2.Dot(v1, v2);

        var invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }
}