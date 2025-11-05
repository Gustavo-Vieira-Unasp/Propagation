using Raylib_cs;
using System.Numerics;
using GameSystem.State;
using System;
using Views.Helper;
using Views.Renderer.Menu.Resolution;
namespace Views.Renderer.Menu.Game;

public class GameMenuRenderer
{
    private readonly GameStateSystem _gameStateSystem;
    private readonly ResolutionMenuRenderer _resolutionMenuRenderer;
    private GameMenuSubState _subState = GameMenuSubState.Main;
    private const int ButtonWidth = 300;
    private const int ButtonHeight = 60;
    private const int Spacing = 80;

    public GameMenuRenderer(GameStateSystem gameStateSystem, ResolutionMenuRenderer resolutionMenuRenderer)
    {
        _gameStateSystem = gameStateSystem;
        _resolutionMenuRenderer = resolutionMenuRenderer;
    }

    public void Render()
    {
        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();
        
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 180));
        
        if (_subState == GameMenuSubState.Resolution) 
        {
            _resolutionMenuRenderer.Render(SetSubState, "PAUSED RESOLUTION SETTINGS", Color.RayWhite);
            return;
        }
        
        string title = "PAUSED - OPTIONS";
        int titleSize = 40;
        int spacing = 80;

        int startX = MenuHelper.GetCenteredX(ButtonWidth);
        int startY = 100 + titleSize + spacing / 2;
        int currentY = startY;

        Raylib.DrawText(title, screenWidth / 2 - Raylib.MeasureText(title, titleSize) / 2, 100, titleSize, Color.RayWhite);

        currentY = MenuHelper.RenderButton(
            "RESUME GAME (ESC)",
            new Rectangle(startX, currentY, ButtonWidth, ButtonHeight),
            Color.DarkGreen, Color.Lime,
            () => _gameStateSystem.CurrentState = GameState.Playing
        );

        currentY = MenuHelper.RenderButton(
            "RESOLUTION",
            new Rectangle(startX, currentY, ButtonWidth, ButtonHeight),
            Color.Blue, Color.SkyBlue,
            () => _subState = GameMenuSubState.Resolution
        );
        
        currentY = MenuHelper.RenderButton(
            "MAIN MENU",
            new Rectangle(startX, currentY, ButtonWidth, ButtonHeight),
            Color.Orange, Color.Yellow,
            () => {
                _gameStateSystem.CurrentState = GameState.MainMenu;
                _subState = GameMenuSubState.Main;
            }
        );
        
        MenuHelper.RenderButton(
            "EXIT GAME",
            new Rectangle(startX, currentY, ButtonWidth, ButtonHeight),
            Color.Maroon, Color.Red,
            () => Raylib.CloseWindow()
        );
    }

    private void SetSubState(GameMenuSubState state)
    {
        _subState = state;
    }
    
    public bool HandleEscape()
    {
        if (_subState == GameMenuSubState.Resolution)
        {
            _subState = GameMenuSubState.Main;
            return true;
        }

        return false;
    }
}