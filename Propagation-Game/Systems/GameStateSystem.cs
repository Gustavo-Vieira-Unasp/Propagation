using System.Collections.Generic;
using System.Numerics;
namespace GameSystem.State;

public enum GameState
{
    MainMenu,
    Playing,
    Paused
}

public class GameStateSystem
{
    public GameState CurrentState { get; set; } = GameState.MainMenu;
    public static readonly List<Vector2> AvaliableResolutions = new List<Vector2>
    {
        new Vector2(800, 600),
        new Vector2(1024, 768),
        new Vector2(1280, 720),
        new Vector2(1920, 1080)
    };
}