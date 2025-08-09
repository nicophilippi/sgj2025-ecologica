using Godot;
using NewGameProject.sim;
using NewGameProject.sim.creature;

public partial class SheepCard : SetCard
{
    [Export] public int HowMany = 60;
    
    public override void Set(Vector2I where)
    {
        Simulation.SetCreatureCell(where, new SheepCell(HowMany));
    }
}