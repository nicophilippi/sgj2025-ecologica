using Godot;
using System;

public partial class PauseButton : TextureButton
{
    [Export] private Texture2D _pausedTex;
    [Export] private Texture2D _unpausedTex;

    [Export] private TextureButton _increaseSpeed;
    [Export] private TextureButton _decreaseSpeed;


    private float[] _timeSteps =
    {
        0.1f,
        0.25f,
        0.5f,
        1,
    };

    private int _currentTimeStep = 3;
    

    private SimTimer timer;
    public override void _EnterTree()
    {
        base._EnterTree();
        timer = new SimTimer();
        timer.WaitTime = _timeSteps[_currentTimeStep];
        AddChild(timer);
        timer.ChangeTimer();
        Pressed += TogglePause;
        _increaseSpeed.Pressed += IncrementSpeed;
        _decreaseSpeed.Pressed += DecrementSpeed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // if (Input.IsActionJustPressed("pause")) TogglePause();
    }


    private void TogglePause()
    {
        GD.Print("Paused");
        timer.ChangeTimer();
        TextureNormal = timer.TimeLeft > 0
            ? _unpausedTex
            : _pausedTex;
    }


    private void IncrementSpeed()
    {
        GD.Print("Increment");
        _currentTimeStep -= 1;
        if (_currentTimeStep < 0) _currentTimeStep = 0;
        timer.WaitTime = _timeSteps[_currentTimeStep];
        timer.Start();
    }

    private void DecrementSpeed()
    {
        GD.Print("Decrement");
        _currentTimeStep += 1;
        if (_currentTimeStep >= _timeSteps.Length) _currentTimeStep = _timeSteps.Length - 1;
        timer.WaitTime = _timeSteps[_currentTimeStep];
        timer.Start();
    }
}
