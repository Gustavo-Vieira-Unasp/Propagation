using Raylib_cs;
using System.Collections.Generic;
using Interfaces.Grid;
namespace Views.Grid;

public class GridRenderer
{
    public void Draw(List<IGridCell> cells)
    {
        Raylib.ClearBackground(Color.Black); 

        foreach (var cell in cells)
        {
            Raylib.DrawTriangle(cell.A, cell.B, cell.C, cell.CurrentColor);
            Raylib.DrawTriangleLines(cell.A, cell.B, cell.C, Color.White); 
        }
    }
}