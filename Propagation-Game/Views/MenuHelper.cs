using Raylib_cs;
using System;
using System.Numerics;
namespace Views.Helper;

public static class MenuHelper
{
    public static int RenderButton(string text, Rectangle rect, Color idleColor, Color hoverColor, Action onClick, int fontSize = 25)
    {
        Color buttonColor = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect)
                            ? hoverColor
                            : idleColor;

        Raylib.DrawRectangleRec(rect, buttonColor);

        int textWidth = Raylib.MeasureText(text, fontSize);

        Raylib.DrawText(
            text,
            (int)rect.X + (int)(rect.Width - textWidth) / 2,
            (int)rect.Y + (int)(rect.Height - fontSize) / 2,
            fontSize,
            Color.White
        );

        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
        {
            onClick.Invoke();
        }

        const int Padding = 20;
        return (int)rect.Y + (int)rect.Height + Padding;
    }

    public static int GetCenteredX(int buttonWidth)
    {
        return (Raylib.GetScreenWidth() - buttonWidth) / 2;
    }
}