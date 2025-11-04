using Raylib_cs;
using System.Numerics;
using System.Grid;
using Views.Grid;
namespace Core.Game;

public class GameLoop
{
    private const int ScreenWidth = 1200;
    private const int ScreenHeight = 800;
    private readonly GridSystem _gridSystem = new();
    private readonly GridRenderer _renderer = new();

    public void Run()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Propagation 0.000.01");
        Raylib.SetTargetFPS(60);

        _gridSystem.GenerateGrid(ScreenWidth, ScreenHeight);

        while (!Raylib.WindowShouldClose())
        {
            HandleInput();
            Render();
        }

        Raylib.CloseWindow();
    }

    private void HandleInput()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            var mousePosition = Raylib.GetMousePosition();
            
            _gridSystem.TryToggleCell(mousePosition);
        }
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        
        _renderer.Draw(_gridSystem.Cells);

        Raylib.EndDrawing();
    }
}