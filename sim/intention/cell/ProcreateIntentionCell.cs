using System.Collections.Generic;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.intention.cell;

public struct ProcreateIntentionCell()
{
    private List<ProcreateIntention> Intentions { get; } = [];

    public void AddProcreateIntention(ProcreateIntention intention) => Intentions.Add(intention);

    public void DeconflictProcreateIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        ProcreateIntentionCell[,] intentionLayer
    )
    {
    }
}