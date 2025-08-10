using Godot;
using NewGameProject.sim;

public partial class SimTimer : Godot.Timer
{
    private bool running = false;
    public override void _EnterTree()
    {
        base._EnterTree();
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
        Simulation.OnTick();
    }
    
    
}
