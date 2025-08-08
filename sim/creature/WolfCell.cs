using NewGameProject.sim.intention;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.creature;

public class WolfCell(int quantity, int hunger) : CreatureCell(quantity, hunger)
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