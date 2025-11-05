using Raylib_cs;
using System.Numerics;
using GameSystem.Camera;
using GameSystem.Grid;
using GameSystem.State;
using Views.Grid;
using Views.Renderer.Menu;
using Views.Renderer.Menu.Game;
using Views.Renderer.Menu.Resolution;
namespace Core.Game;

public class GameLoop
{
    private const int InitialScreenWidth = 1200;
    private const int InitialScreenHeight = 800;
    private readonly CameraSystem _cameraSystem;
    private readonly GridSystem _gridSystem = new();
    private readonly GridRenderer _gridRenderer = new();
    private readonly GameStateSystem _gameStateSystem = new();
    private readonly MainMenuRenderer _mainMenuRenderer;
    private readonly GameMenuRenderer _gameMenuRenderer;
    private readonly ResolutionMenuRenderer _resolutionMenuRenderer;
    private bool _isGamePreloaded = false;

    public GameLoop()
    {
        _cameraSystem = new CameraSystem(InitialScreenWidth, InitialScreenHeight);
        _resolutionMenuRenderer = new ResolutionMenuRenderer(_gameStateSystem, _cameraSystem);
        _mainMenuRenderer = new MainMenuRenderer(_gameStateSystem, _resolutionMenuRenderer);
        _gameMenuRenderer = new GameMenuRenderer(_gameStateSystem, _resolutionMenuRenderer);
    }

    public void Run()
    {
        Raylib.InitWindow(InitialScreenWidth, InitialScreenHeight, "Propagation - Ver.:0.000.01");
        Raylib.SetTargetFPS(60);
        Raylib.SetExitKey(KeyboardKey.Null);

        PreoloadAssets(InitialScreenWidth, InitialScreenHeight);

        while (!Raylib.WindowShouldClose())
        {
            HandleSystemInput();

            if (_gameStateSystem.CurrentState == GameState.Playing)
            {
                HandleInput();
                _cameraSystem.Update();
            }

            Render();
        }

        Raylib.CloseWindow();
    }

    private void PreoloadAssets(int width, int height)
    {
        _gridSystem.GenerateGrid(width, height);

        _isGamePreloaded = true;
    }

    private void HandleSystemInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            if (_gameStateSystem.CurrentState == GameState.Playing)
            {
                _gameStateSystem.CurrentState = GameState.Paused;
            }
            else if (_gameStateSystem.CurrentState == GameState.Paused)
            {
                if (_gameMenuRenderer.HandleEscape())
                {
                    return;
                }

                _gameStateSystem.CurrentState = GameState.Playing;
            }
            else if (_gameStateSystem.CurrentState == GameState.MainMenu)
            {
                Raylib.CloseWindow();
            }
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

        if (!_isGamePreloaded)
        {
            Raylib.DrawText("LOADING...", 100, 100, 30, Color.White);
        }
        else if (_gameStateSystem.CurrentState == GameState.MainMenu)
        {
            _mainMenuRenderer.Render();
        }
        else
        {
            Raylib.BeginMode2D(_cameraSystem.Camera);
            _gridRenderer.Draw(_gridSystem.Cells);
            Raylib.EndMode2D();
        }

        if (_gameStateSystem.CurrentState == GameState.Paused)
        {
            _gameMenuRenderer.Render();
        }

        Raylib.EndDrawing();
    }
}