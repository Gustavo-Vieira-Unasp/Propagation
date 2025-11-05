using Raylib_cs;
using System.Numerics;
using GameSystem.State;
using GameSystem.Camera;
using System;
namespace Views.Renderer.Menu.Resolution;

public class ResolutionMenuRenderer
{
    private readonly GameStateSystem _gameStateSystem;
    private readonly CameraSystem _cameraSystem;
    private Action<GameMenuSubState>? _setSubStateCallback;

    public ResolutionMenuRenderer(GameStateSystem gameStateSystem, CameraSystem cameraSystem)
    {
        _gameStateSystem = gameStateSystem;
        _cameraSystem = cameraSystem;
    }

    public void Render(Action<GameMenuSubState> setSubStateCallback, string titleText, Color titleColor)
    {
        _setSubStateCallback = setSubStateCallback;

        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();

        int titleSize = 35;
        Raylib.DrawText(titleText, screenWidth / 2 - Raylib.MeasureText(titleText, titleSize) / 2, 100, titleSize, titleColor);

        int startY = 180;
        int buttonWidth = 300;
        int buttonHeight = 50;
        int spacing = 60;
        
        for (int i = 0; i < GameStateSystem.AvaliableResolutions.Count; i++)
        {
            Vector2 res = GameStateSystem.AvaliableResolutions[i];
            string resText = $"{res.X}x{res.Y}";
            Rectangle buttonRect = new Rectangle(screenWidth / 2 - buttonWidth / 2, startY + (i * spacing) + spacing, buttonWidth, buttonHeight);

            RenderButton(
                resText,
                buttonRect,
                Color.Blue, Color.SkyBlue,
                () => ApplyNewResolution((int)res.X, (int)res.Y)
            );
        }
        
        RenderButton(
            "BACK",
            new Rectangle(screenWidth / 2 - buttonWidth / 2, startY + (GameStateSystem.AvaliableResolutions.Count + 1) * spacing, buttonWidth, buttonHeight),
            Color.DarkGray, Color.Gray,
            () => _setSubStateCallback(GameMenuSubState.Main)
        );
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

    private void ApplyNewResolution(int newWidth, int newHeight)
    {
        if (newWidth <= 0 || newHeight <= 0) return;

        Raylib.SetWindowSize(newWidth, newHeight);
        _cameraSystem.UpdateScreenSize(newWidth, newHeight);
        
        _setSubStateCallback?.Invoke(GameMenuSubState.Main); 
    }
}