using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.plant;

namespace NewGameProject.sim.terrain;

public class PlainsCell(int height, int temperature) : TerrainCell(height, temperature)
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