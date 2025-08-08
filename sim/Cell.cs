using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim;

public abstract class Cell
{
    /**
     * Based on state of this cell, compute intentions of what to do to the surrounding cells.
     */
    public abstract void ComputeMoveIntentions(
        int x,
        int y,
        TerrainCell[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        IntentionCell[,] intentionLayer
    );
}