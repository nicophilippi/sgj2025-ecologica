using System.Collections.Generic;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.intention.cell;

public struct MoveIntentionCell()
{
    private List<MoveIntention> Intentions { get; set; } = [];
    
    

    public void DeconflictMoveIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        MoveIntentionCell[,] intentionLayer
    )
    {
        Intentions.Clear();
    }

    public void AddMoveIntention(MoveIntention intention)
    {
        Intentions.Add(intention);
    }
}