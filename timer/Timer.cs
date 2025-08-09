using Godot;

public partial class Sim_Timer : Godot.Timer
{
    private bool running = false;
    private const double TIMER_INTERVAL = 0.07; // Do not use less than 0.05s! May cause issues with physics frame interval
    public override void _EnterTree()
    {
        base._EnterTree();
        base.WaitTime = TIMER_INTERVAL;
        Timeout += Test;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Timeout -= Test;
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
    
    private void Test()
    {
        GD.Print("Hello World!");
    }
    
    
}
