using Godot;
using NewGameProject.sim;
using NewGameProject.sim.creature;

public partial class WolfCard : SetCard
{
    public override void Set(Vector2I where)
    {
        Simulation.SetCreatureCell(where, new WolfCell(20));
    }
}