using Godot;
using System;
using NewGameProject.sim;

public partial class CrownIcon : Sprite2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Simulation.NumOverflow > 0)
        {
            Visible = true;
        }
    }
}
