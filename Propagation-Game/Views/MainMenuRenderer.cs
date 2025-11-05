using Raylib_cs;
using System.Numerics;
using GameSystem.State;
using System;
using Views.Helper;
using Views.Renderer.Menu.Resolution;
namespace Views.Renderer.Menu;

public class MainMenuRenderer
{
    private readonly GameStateSystem _gameStateSystem;
    private readonly ResolutionMenuRenderer _resolutionMenuRenderer;
    private GameMenuSubState _subState = GameMenuSubState.Main;
    private const int ButtonWidth = 300;
    private const int ButtonHeight = 60;
    private const int TitleOffset = 100;

    public MainMenuRenderer(GameStateSystem gameStateSystem, ResolutionMenuRenderer resolutionMenuRenderer)
    {
        _gameStateSystem = gameStateSystem;
        _resolutionMenuRenderer = resolutionMenuRenderer;
    }

    public void Render()
    {
        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();
        
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(50, 50, 50, 255));
        
        if (_subState == GameMenuSubState.Resolution)
        {
            _resolutionMenuRenderer.Render(SetSubState, "MAIN MENU RESOLUTION", Color.RayWhite);
            return;
        }
        
        string title = "PROPAGATION";
        int titleSize = 70;
        Raylib.DrawText(title, screenWidth / 2 - Raylib.MeasureText(title, titleSize) / 2, 100, titleSize, Color.Gold);

        int startX = MenuHelper.GetCenteredX(ButtonWidth);
        int menuBlockHeight = 3 * ButtonHeight + 2 * 20;
        int currentY = screenHeight / 2 - menuBlockHeight / 2 + 50;

        currentY = MenuHelper.RenderButton(
            "PLAY",
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
            "EXIT",
            new Rectangle(startX, currentY, ButtonWidth, ButtonHeight),
            Color.Maroon, Color.Red,
            () => Raylib.CloseWindow()
        );
    }

    private void SetSubState(GameMenuSubState state)
    {
        _subState = state;
    }

    private void RenderButton(string text, Rectangle rect, Color idleColor, Color hoverColor, Action onClick)
    {
        Color buttonColor = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect) 
                            ? hoverColor 
                            : idleColor;

        Raylib.DrawRectangleRec(rect, buttonColor);
        int fontSize = 25;
        if (text.Length > 20) fontSize = 20;

        Raylib.DrawText(text, (int)rect.X + (int)rect.Width / 2 - Raylib.MeasureText(text, fontSize) / 2, (int)rect.Y + ((int)rect.Height - fontSize) / 2, fontSize, Color.White);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
        {
            onClick.Invoke();
        }
    }
}