using Raylib_cs;
using System.Numerics;
using GameSystem.Camera;
using GameSystem.Grid;
using Views.Grid;
namespace Core.Game;

public class GameLoop
{
    private const int ScreenWidth = 1200;
    private const int ScreenHeight = 800;
    private readonly CameraSystem _cameraSystem;
    private readonly GridSystem _gridSystem = new();
    private readonly GridRenderer _renderer = new();

    public GameLoop()
    {
        _cameraSystem = new CameraSystem(ScreenWidth, ScreenHeight);
    }

    public void Run()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Propagation 0.000.01");
        Raylib.SetTargetFPS(60);

        _gridSystem.GenerateGrid(ScreenWidth, ScreenHeight);

        while (!Raylib.WindowShouldClose())
        {
            HandleInput();

            _cameraSystem.Update();

            Render();
        }

        Raylib.CloseWindow();
    }

    private void HandleInput()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 worldMousePosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _cameraSystem.Camera);

            _gridSystem.TryToggleCell(worldMousePosition);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            _cameraSystem.ResetCamera();
        }
    }

    private void Render()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Black);

        Raylib.BeginMode2D(_cameraSystem.Camera);

        _renderer.Draw(_gridSystem.Cells);

        Raylib.EndMode2D();

        Raylib.EndDrawing();
    }
}