using Godot;
using System;
using NewGameProject.sim;

public partial class PointsText : RichTextLabel
{
    public override void _EnterTree()
    {
        base._EnterTree();
        Text = $"Points: {Simulation.SimulationPoints}";
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        Text = $"Points: {Simulation.SimulationPoints}";
    }
    
}
