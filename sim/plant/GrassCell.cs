using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.plant;

public class GrassCell(int health) : PlantCell(health)
{
    public override void ComputeMoveIntentions(
        int x,
        int y,
        TerrainCell[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        IntentionCell[,] intentionLayer
    )
    {
        
    }
}