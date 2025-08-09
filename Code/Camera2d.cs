using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export] private Vector2 _zoomSpeed = Vector2.One;
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("zoom in"))
        {
            Zoom -= _zoomSpeed * (float) delta;
        } else if (Input.IsActionJustPressed("zoom out"))
        {
            Zoom += _zoomSpeed * (float) delta;
        }
    }
}
