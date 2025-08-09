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

                PlantCellType
                    plantCellTypeA = floraLayer[fromPositionA.X, fromPositionA.Y].Type,
                    plantCellTypeB = floraLayer[fromPositionB.X, fromPositionB.Y].Type;

                return PlantCellTypeUtil.ProcreatePriority(plantCellTypeA) -
                       PlantCellTypeUtil.ProcreatePriority(plantCellTypeB);
            },
            SimulationLayer.Fauna => (intentionA, intentionB) =>
            {
                SimulationPosition
                    fromPositionA = intentionA.FromPosition,
                    fromPositionB = intentionB.FromPosition;

                CreatureCellType
                    creatureCellTypeA = faunaLayer[fromPositionA.X, fromPositionA.Y].Type,
                    creatureCellTypeB = faunaLayer[fromPositionB.X, fromPositionB.Y].Type;

                return CreatureCellTypeUtil.ProcreatePriority(creatureCellTypeA) -
                       CreatureCellTypeUtil.ProcreatePriority(creatureCellTypeB);
            },
            _ => throw new Exception("")
        };
        
        Intentions.Sort(comparison);

        ProcreateIntention winnerIntention = Intentions[^1];

        switch (layer)
        {
            case SimulationLayer.Flora:
                PlantCell winnerPlantCell = floraLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y];
                
                floraLayer[x, y] = winnerPlantCell.Type switch
                {
                    PlantCellType.Grass => new GrassCell(),
                    PlantCellType.Barren => new BarrenCell(),
                    _ => throw new ArgumentOutOfRangeException("")
                };
                
                Intentions.Clear();
                break;
            case SimulationLayer.Fauna:
                CreatureCell winnerCreatureCell = faunaLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y];

                faunaLayer[x, y] = winnerCreatureCell.Type switch
                {
                    CreatureCellType.Sheep => new SheepCell(1),
                    CreatureCellType.Empty => new EmptyCell(),
                    _ => throw new ArgumentOutOfRangeException("")
                };
                
                Intentions.Clear();
                break;
            default:
                throw new Exception("");
        }
    }
}