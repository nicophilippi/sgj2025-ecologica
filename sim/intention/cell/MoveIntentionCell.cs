using System;
using System.Collections.Generic;
using Godot;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.intention.cell;

public struct MoveIntentionCell()
{
    private List<MoveIntention> Intentions { get; } = [];
    
    public void DeconflictMoveIntentions(
        int x,
        int y,
        SimulationLayer layer,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        MoveIntentionCell[,] intentionLayer
    )
    {
        // No intentions to deconflict
        if (Intentions.Count == 0)
        {
            return;
        }
        
        // Only creatures can move, so other layers don't need move deconflicting
        if (layer != SimulationLayer.Fauna)
        {
            Intentions.Clear();
            return;
        }

        // Sort intentions by move priority (high number = high priority)
        Intentions.Sort((intentionA, intentionB) =>
        {
            SimulationPosition
                fromPositionA = intentionA.FromPosition,
                fromPositionB = intentionB.FromPosition;

            CreatureCell
                creatureCellA = faunaLayer[fromPositionA.X, fromPositionA.Y],
                creatureCellB = faunaLayer[fromPositionB.X, fromPositionB.Y];
            
            return CreatureCellTypeUtil.MovePriority(creatureCellA) - CreatureCellTypeUtil.MovePriority(creatureCellB);
        });

        MoveIntention winnerIntention = Intentions[^1];
        CreatureCell winnerCreatureCell = faunaLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y];

        if (winnerCreatureCell == null)
        {
            throw new NullReferenceException("Winner creature cell was null when deconflicting move intentions!");
        }

        winnerCreatureCell.Quantity -= winnerIntention.Quantity;
        if (winnerCreatureCell.Quantity == 0)
        {
            faunaLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y] = null;
        }

        faunaLayer[x, y] = winnerCreatureCell.Type switch
        {
            CreatureCellType.Sheep => new SheepCell(winnerIntention.Quantity),
            _ => throw new Exception("")
        };
            
        Intentions.Clear();
    }

    public void AddMoveIntention(MoveIntention intention)
    {
        Intentions.Add(intention);
    }
}