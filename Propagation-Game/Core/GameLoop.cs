using Raylib_cs;
using System.Numerics;
using GameSystem.Camera;
using GameSystem.Grid;
using GameSystem.State;
using Views.Grid;
using Views.UIRender;
namespace Core.Game;

public class GameLoop
{
    private const int InitialScreenWidth = 1200;
    private const int InitialScreenHeight = 800;
    private readonly CameraSystem _cameraSystem;
    private readonly GridSystem _gridSystem = new();
    private readonly GridRenderer _renderer = new();
    private readonly GameStateSystem _gameStateSystem = new();
    private readonly UIRenderer _uiRenderer;

    public GameLoop()
    {
        _cameraSystem = new CameraSystem(InitialScreenWidth, InitialScreenHeight);
        _uiRenderer = new UIRenderer(_gameStateSystem, _cameraSystem);
    }

    public void Run()
    {
        Raylib.InitWindow(InitialScreenWidth, InitialScreenHeight, "Propagation - Ver.:0.000.01");
        Raylib.SetTargetFPS(60);

        Raylib.SetExitKey(KeyboardKey.Null);

        _gridSystem.GenerateGrid(InitialScreenWidth, InitialScreenHeight);

        while (!Raylib.WindowShouldClose())
        {
            if (_gameStateSystem.CurrentState == GameState.Playing)
            {
                HandleInput();
                _cameraSystem.Update();
            }

            HandleSystemInput();

            Render();
        }

        Raylib.CloseWindow();
    }

    private void HandleSystemInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            _gameStateSystem.CurrentState = (_gameStateSystem.CurrentState == GameState.Playing)
                ? GameState.Paused
                : GameState.Playing;
        }
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

        if (_gameStateSystem.CurrentState == GameState.Paused)
        {
            _uiRenderer.RenderPauseMenu();
        }

        Raylib.EndDrawing();
    }
}