using Godot;
using System;

public partial class PauseButton : TextureButton
{
    [Export] private Texture2D _pausedTex;
    [Export] private Texture2D _unpausedTex;


    private SimTimer timer;
    public override void _EnterTree()
    {
        base._EnterTree();
        timer = new SimTimer();
        AddChild(timer);
        timer.ChangeTimer();
        Pressed += TogglePause;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("pause")) TogglePause();
    }


    private void TogglePause()
    {
        GD.Print("Paused");
        timer.ChangeTimer();
        TextureNormal = timer.TimeLeft > 0
            ? _unpausedTex
            : _pausedTex;
    }
}
