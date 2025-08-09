using System.Collections.Generic;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.intention.cell;

public struct EatIntentionCell()
{
    private List<EatIntention> Intentions { get; set; } = [];

    public void DeconflictEatIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        EatIntentionCell[,] intentionLayer
    )
    {
    }
}