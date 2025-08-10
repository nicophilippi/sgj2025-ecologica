using Godot;
using NewGameProject.sim;

public partial class SimTimer : Godot.Timer
{
    private bool running = false;
    private const double TIMER_INTERVAL = 1; // Do not use less than 0.05s! May cause issues with physics frame interval
    public override void _EnterTree()
    {
        base._EnterTree();
        base.WaitTime = TIMER_INTERVAL;
        Timeout += Tick;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Timeout -= Tick;
    }

    public void ChangeTimer()
    {
        if (base.TimeLeft > 0)
        {
            base.Stop();
        } 
		else 
		{	
			base.Start();
		}
    }
    
    private void Tick()
    {
        Simulation.OnTick();
    }
    
    
}
