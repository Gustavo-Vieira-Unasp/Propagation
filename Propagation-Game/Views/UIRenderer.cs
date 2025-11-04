using Raylib_cs;
using System.Numerics;
using GameSystem.State;
using GameSystem.Camera;
using System;

namespace Views.UIRender;

public class UIRenderer
{
    private readonly GameStateSystem _gameStateSystem;
    private readonly CameraSystem _cameraSystem;
    
    public UIRenderer(GameStateSystem gameStateSystem, CameraSystem cameraSystem)
    {
        _gameStateSystem = gameStateSystem;
        _cameraSystem = cameraSystem;
    }

    public void RenderPauseMenu()
    {
        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();
        
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 150));
        
        string title = "PAUSE - SETTINGS";
        int titleSize = 40;
        Raylib.DrawText(title, screenWidth / 2 - Raylib.MeasureText(title, titleSize) / 2, 100, titleSize, Color.RayWhite);

        int startY = 200;
        int buttonWidth = 300;
        int buttonHeight = 50;
        int spacing = 60;

        Raylib.DrawText("Change Resolution:", screenWidth / 2 - buttonWidth / 2, startY, 20, Color.RayWhite);

        for (int i = 0; i < GameStateSystem.AvaliableResolutions.Count; i++)
        {
            Vector2 res = GameStateSystem.AvaliableResolutions[i];
            string resText = $"{res.X}x{res.Y}";
            Rectangle buttonRect = new Rectangle(screenWidth / 2 - buttonWidth / 2, startY + (i + 1) * spacing, buttonWidth, buttonHeight);

            Color buttonColor = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), buttonRect) 
                                ? Color.SkyBlue 
                                : Color.Blue;

            Raylib.DrawRectangleRec(buttonRect, buttonColor);
            Raylib.DrawText(resText, (int)buttonRect.X + buttonWidth / 2 - Raylib.MeasureText(resText, 25) / 2, (int)buttonRect.Y + 12, 25, Color.White);

            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), buttonRect))
            {
                ApplyNewResolution((int)res.X, (int)res.Y);
            }
        }
        
        string resumeText = "Resume Game (ESC)";
        Rectangle resumeRect = new Rectangle(screenWidth / 2 - buttonWidth / 2, startY + (GameStateSystem.AvaliableResolutions.Count + 1) * spacing + 50, buttonWidth, buttonHeight);
        
        Color resumeColor = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), resumeRect) ? Color.Green : Color.DarkGreen;

        Raylib.DrawRectangleRec(resumeRect, resumeColor);
        Raylib.DrawText(resumeText, (int)resumeRect.X + buttonWidth / 2 - Raylib.MeasureText(resumeText, 25) / 2, (int)resumeRect.Y + 12, 25, Color.White);
        
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), resumeRect))
        {
             _gameStateSystem.CurrentState = GameState.Playing;
        }
    }
    
    private void ApplyNewResolution(int newWidth, int newHeight)
    {
        if (newWidth <= 0 || newHeight <= 0) return;

        Raylib.SetWindowSize(newWidth, newHeight);

        _cameraSystem.UpdateScreenSize(newWidth, newHeight);
    }
}