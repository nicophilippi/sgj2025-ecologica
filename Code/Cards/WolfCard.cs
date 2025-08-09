using Godot;
using NewGameProject.sim;
using NewGameProject.sim.creature;

public partial class WolfCard : SetCard
{
    [Export] public int HowMany;


    public override void Set(Vector2I where)
    {
        Simulation.SetCreatureCell(where, new WolfCell(HowMany));
    }
}