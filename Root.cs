using Godot;
using System;

public partial class Root : Node
{
    private Sim_Timer timer;
    public override void _EnterTree()
    {
        base._EnterTree();
        timer = new Sim_Timer();
        AddChild(timer);
        timer.ChangeTimer();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("pause"))
        {
            GD.Print("Paused");
            timer.ChangeTimer();
        }
    }
}
