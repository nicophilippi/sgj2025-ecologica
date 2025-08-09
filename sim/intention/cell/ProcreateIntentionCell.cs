using System;
using System.Collections.Generic;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.intention.cell;

public struct ProcreateIntentionCell()
{
    private List<ProcreateIntention> Intentions { get; } = [];

    public void AddProcreateIntention(ProcreateIntention intention) => Intentions.Add(intention);

    public void DeconflictProcreateIntentions(
        int x,
        int y,
        SimulationLayer layer,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        ProcreateIntentionCell[,] intentionLayer
    )
    {
        // No intentions to deconflict
        if (Intentions.Count == 0)
        {
            return;
        }
        
        // Terra layer can not procreate
        if (layer == SimulationLayer.Terra)
        {
            Intentions.Clear();
            return;
        }
        
        // Sort intentions by move priority (high number = high priority)
        Comparison<ProcreateIntention> comparison = layer switch
        {
            SimulationLayer.Flora => (intentionA, intentionB) =>
            {
                SimulationPosition
                    fromPositionA = intentionA.FromPosition,
                    fromPositionB = intentionB.FromPosition;

                PlantCell
                    plantCellA = floraLayer[fromPositionA.X, fromPositionA.Y],
                    plantCellB = floraLayer[fromPositionB.X, fromPositionB.Y];

                return PlantCellTypeUtil.ProcreatePriority(plantCellA) - PlantCellTypeUtil.ProcreatePriority(plantCellB);
            },
            SimulationLayer.Fauna => (intentionA, intentionB) =>
            {
                SimulationPosition
                    fromPositionA = intentionA.FromPosition,
                    fromPositionB = intentionB.FromPosition;

                CreatureCell
                    creatureCellA = faunaLayer[fromPositionA.X, fromPositionA.Y],
                    creatureCellB = faunaLayer[fromPositionB.X, fromPositionB.Y];

                return CreatureCellTypeUtil.ProcreatePriority(creatureCellA) - CreatureCellTypeUtil.ProcreatePriority(creatureCellB);
            },
            _ => throw new ArgumentOutOfRangeException("")
        };
        
        Intentions.Sort(comparison);

        ProcreateIntention winnerIntention = Intentions[^1];

        switch (layer)
        {
            case SimulationLayer.Flora:
                PlantCell winnerPlantCell = floraLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y];

                if (winnerPlantCell == null)
                {
                    throw new NullReferenceException("Winner plant cell was null when deconflicting procreate intentions!");
                }
                
                floraLayer[x, y] = winnerPlantCell.Type switch
                {
                    PlantCellType.Grass => new GrassCell(),
                    _ => throw new ArgumentOutOfRangeException("")
                };
                
                Intentions.Clear();
                break;
            case SimulationLayer.Fauna:
                CreatureCell winnerCreatureCell = faunaLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y];

                if (winnerCreatureCell == null)
                {
                    throw new NullReferenceException("Winner creature cell was null when deconflicting procreate intentions!");
                }

                faunaLayer[x, y] = winnerCreatureCell.Type switch
                {
                    CreatureCellType.Sheep => new SheepCell(1),
                    _ => throw new ArgumentOutOfRangeException("")
                };
                
                Intentions.Clear();
                break;
            default:
                throw new Exception("");
        }
    }
}