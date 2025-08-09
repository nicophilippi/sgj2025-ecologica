using Godot;
using NewGameProject.sim;
using NewGameProject.sim.plant;

public partial class FullGrassCard : SetCard
{
    public override void Set(Vector2I where)
    {
        Simulation.SetPlantCell(where, new GrassCell
        {
            Health = GrassCell.MAX_HEALTH,
        });
    }
}