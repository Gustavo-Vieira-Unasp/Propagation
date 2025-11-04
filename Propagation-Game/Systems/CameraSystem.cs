using Raylib_cs;
using System;
using System.Numerics;

namespace GameSystem.Camera;

public class CameraSystem
{
    private Camera2D _camera;
    public Camera2D Camera => _camera;
    private const float MinZoom = 0.7f;
    private const float MaxZoom = 18.0f;
    private const float InitialZoom = 2.5f;
    private const float CameraSpeed = 10f;
    private const float PanMargin = 200f; 
    private readonly Vector2 _screenCenter;
    private const int WorldWidth = 1200;
    private const int WorldHeight = 800;
    private static readonly Vector2 WorldCenter = new Vector2(WorldWidth / 2f, WorldHeight / 2f);

    public CameraSystem(int screenWidth, int screenHeight)
    {
        _screenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);

        _camera = new Camera2D
        {
            Offset = _screenCenter,
            Target = WorldCenter,
            Rotation = 0.0f,
            Zoom = InitialZoom
        };
    }

    private void LimitCameraTarget()
    {
        float visibleWidth = WorldWidth / _camera.Zoom;
        float visibleHeight = WorldHeight / _camera.Zoom;
        
        float marginX = visibleWidth / 2f;
        float marginY = visibleHeight / 2f;
        
        float minTargetX = marginX - PanMargin;
        float maxTargetX = WorldWidth - marginX + PanMargin; 

        float minTargetY = marginY - PanMargin;
        float maxTargetY = WorldHeight - marginY + PanMargin;
        
        if (minTargetX > maxTargetX || _camera.Zoom <= 1.0f)
        {
            _camera.Target.X = WorldCenter.X;
        }
        else 
        {
            _camera.Target.X = Math.Clamp(_camera.Target.X, minTargetX, maxTargetX);
        }

        if (minTargetY > maxTargetY || _camera.Zoom <= 1.0f)
        {
            _camera.Target.Y = WorldCenter.Y;
        }
        else
        {
            _camera.Target.Y = Math.Clamp(_camera.Target.Y, minTargetY, maxTargetY);
        }
    }

    public void Update()
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            Vector2 delta = Raylib.GetMouseDelta();
            delta = Vector2.Divide(delta, _camera.Zoom);
            _camera.Target = Vector2.Subtract(_camera.Target, delta);
        }

        Vector2 movement = Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W)) { movement.Y -= 1.0f; }
        if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S)) { movement.Y += 1.0f; }
        if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) { movement.X -= 1.0f; }
        if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) { movement.X += 1.0f; }
        
        if (movement != Vector2.Zero)
        {
            movement = Vector2.Normalize(movement);
            
            movement *= CameraSpeed * Raylib.GetFrameTime() * 60; 
            _camera.Target = Vector2.Add(_camera.Target, movement);
        }

        float wheel = Raylib.GetMouseWheelMove();
        if (wheel != 0)
        {
            float zoomFactor = 1.1f;
            if (wheel > 0) { _camera.Zoom *= zoomFactor; }
            else { _camera.Zoom /= zoomFactor; }
            _camera.Zoom = Math.Clamp(_camera.Zoom, MinZoom, MaxZoom);
        }

        LimitCameraTarget();
    }
    
    public void ResetCamera()
    {
        _camera.Target = WorldCenter;
        _camera.Zoom = InitialZoom;
    }
}