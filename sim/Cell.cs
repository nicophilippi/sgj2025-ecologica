using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim;

public abstract class Cell
{
    public abstract void ComputeMoveIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        MoveIntentionCell[,] intentionLayer
    );
    
    public abstract void ComputeEatIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        EatIntentionCell[,] intentionLayer
    );
    
    public abstract void ComputeProcreateIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        ProcreateIntentionCell[,] intentionLayer
    );
}