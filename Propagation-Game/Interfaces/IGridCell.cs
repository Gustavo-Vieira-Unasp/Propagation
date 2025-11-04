using Raylib_cs;
using System.Numerics;
namespace Interfaces.Grid;

public interface IGridCell
{
    Vector2 A { get; }
    Vector2 B { get; }
    Vector2 C { get; }
    bool IsBuilt { get; set; }
    Color CurrentColor { get; }

    bool Contains(Vector2 p);
}