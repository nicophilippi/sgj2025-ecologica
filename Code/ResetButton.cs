using Godot;
using System;
using NewGameProject.sim;

public partial class ResetButton : TextureButton
{
    public override void _Pressed()
    {
        base._Pressed();
        Simulation.resetSimulation();    
    }
    
}
