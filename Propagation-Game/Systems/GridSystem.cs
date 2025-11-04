using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using Interfaces.Grid;
using Models.Grid;
namespace System.Grid;

public class GridSystem
{
    private const float SideLength = 50f;
    private static readonly float Height = SideLength * (float)Math.Sqrt(3) / 2;
    private static readonly float GridStepX = SideLength / 2;
    private static readonly float GridStepY = Height;

    public List<IGridCell> Cells { get; private set; } = new();

    public void GenerateGrid(int ScreenWidth, int ScreenHeight)
    {
        Cells.Clear();

        float xStartOffset = -SideLength;
        float yStartOffset = -Height;

        int numCols = (int)(ScreenWidth / SideLength) * 2 + 8;
        int numRows = (int)(ScreenHeight / Height) + 5;

        for (int row = 0; row < numRows; row++)
        {
            float yBase = row * GridStepY + yStartOffset;
            float xRowOffset = (row % 2 == 1) ? GridStepX : 0;

            for (int col = 0; col < numCols; col++)
            {
                float xBase = col * GridStepX + xRowOffset + xStartOffset;
                bool isUp = (col % 2 == 0);

                Vector2 A, B, C;
                if (isUp)
                {
                    A = new Vector2(xBase, yBase + Height);
                    B = new Vector2(xBase + SideLength, yBase + Height);
                    C = new Vector2(xBase + GridStepX, yBase);
                }
                else
                {
                    A = new Vector2(xBase, yBase);
                    B = new Vector2(xBase + GridStepX, yBase + Height);
                    C = new Vector2(xBase + SideLength, yBase);
                }

                Cells.Add(new TriangleCell(A, B, C));
            }
        }
    } 

    public void TryToggleCell(Vector2 mousePosition)
    {
        foreach (var cell in Cells)
        {
            if (cell.Contains(mousePosition))
            {
                cell.IsBuilt = !cell.IsBuilt;
                break;
            }
        }
    }
}