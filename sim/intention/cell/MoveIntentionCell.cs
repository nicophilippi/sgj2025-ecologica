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
        var dic = new Dictionary<CreatureCellType, MoveIntention>();

        foreach (var i in Intentions)
        {
            var creatureCell = faunaLayer[i.FromPosition.X, i.FromPosition.Y];
            var maxQuant = creatureCell.MaxQuantity;
            var key = creatureCell.Type;
            if (!dic.TryAdd(key, i))
            {
                dic[key].Quantity += i.Quantity;
                if (dic[key].Quantity > maxQuant) dic[key].Quantity = maxQuant;
            }
        }
        Intentions.Clear();
        Intentions.AddRange(dic.Values);
        
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
        if (winnerCreatureCell.Quantity <= 0)
        {
            faunaLayer[winnerIntention.FromPosition.X, winnerIntention.FromPosition.Y] = null;
        }

        faunaLayer[x, y] = winnerCreatureCell.Type switch
        {
            CreatureCellType.Sheep => new SheepCell(winnerIntention.Quantity),
            CreatureCellType.Wolf => new WolfCell(winnerIntention.Quantity),
            _ => throw new Exception("SHOULDNOTHAPPEN")
        };
            
        Intentions.Clear();
    }

    public void AddMoveIntention(MoveIntention intention)
    {
        if (intention.Quantity < 0) throw new Exception("Negative amount of move");
        Intentions.Add(intention);
    }
}