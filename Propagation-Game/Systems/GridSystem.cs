using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using Interfaces.Grid;
using Models.Grid;
namespace GameSystem.Grid;

public class GridSystem
{
    private const float SideLength = 50f;
    private static readonly float Height = SideLength * (float)Math.Sqrt(3) / 2;
    private static readonly float GridStepX = SideLength / 2;
    private static readonly float GridStepY = Height;
    private const int WorldWidth = 1200;
    private const int WorldHeight = 800;
    private readonly Dictionary<Tuple<int, int>, List<IGridCell>> _spatialHash = new();
    private const float HashCellSize = SideLength * 2;
    public List<IGridCell> Cells { get; private set; } = new();

    public void GenerateGrid(int ScreenWidth, int ScreenHeight)
    {
        Cells.Clear();
        _spatialHash.Clear();

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

                var newCell = new TriangleCell(A, B, C);
                Cells.Add(newCell);

                Vector2 centroid = (A + B + C) / 3f;
                InsertCellIntoHash(newCell, centroid);
            }
        }
    }

    private Tuple<int, int> GetHashKey(Vector2 position)
    {
        int x = (int)Math.Floor(position.X / HashCellSize);
        int y = (int)Math.Floor(position.Y / HashCellSize);
        return new Tuple<int, int>(x, y);
    }
    
    private void InsertCellIntoHash(IGridCell cell, Vector2 centroid)
    {
        Tuple<int, int> key = GetHashKey(centroid);

        if (!_spatialHash.ContainsKey(key))
        {
            _spatialHash.Add(key, new List<IGridCell>());
        }

        _spatialHash[key].Add(cell);
    }

    public void TryToggleCell(Vector2 mousePosition)
    {
        Tuple<int, int> primaryKey = GetHashKey(mousePosition);

        var keysToSearch = new List<Tuple<int, int>>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                keysToSearch.Add(new Tuple<int, int>(primaryKey.Item1 + i, primaryKey.Item2 + j));
            }
        }

        foreach (var key in keysToSearch)
        {
            if (_spatialHash.TryGetValue(key, out List<IGridCell>? potentialCells))
            {
                foreach (var cell in potentialCells)
                {
                    if (cell.Contains(mousePosition))
                    {
                        cell.IsBuilt = !cell.IsBuilt;
                        return;
                    }
                }
            }
        }
    }
}