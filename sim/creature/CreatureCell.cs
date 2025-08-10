using System;
using System.Collections.Generic;
using Godot;
using NewGameProject.sim.intention;
using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.creature;

public abstract class CreatureCell(CreatureCellType type, int quantity, int maxQuantity, int visionRange) : Cell
{
    public CreatureCellType Type { get; } = type;
    
    public int Quantity { get; set; } = quantity;
    public int MaxQuantity { get; } = maxQuantity;
    public int VisionRange { get; } = visionRange;
    
    public override void ComputeMoveIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        MoveIntentionCell[,] intentionLayer
    )
    {
        SimulationPosition
            myPosition = new(x, y),
            bestPosition = myPosition,
            worstPosition = myPosition;

        int
            bestAttractiveness = -9999,
            worstAttractiveness = 9999;

        // Randomly flip the x and y iteration order
        int
            xFlip = Simulation.RandomIntBetween(0, 100) < 50 ? -1 : 1,
            yFlip = Simulation.RandomIntBetween(0, 100) < 50 ? -1 : 1;

        // Remember best and worst positions in range
        for (int currX = x - VisionRange*xFlip; xFlip == 1 ? currX <= x + VisionRange : currX >= x - VisionRange; currX += xFlip)
        {
            for (int currY = y - VisionRange*yFlip; yFlip == 1 ? currY <= y + VisionRange : currY >= y - VisionRange; currY += yFlip)
            {
                // Don't include the current position in evaluation
                if (currX == x && currY == y) continue;
                
                // Don't evaluate positions out of world range
                if (currX is < 0 or >= Simulation.WorldSize || currY is < 0 or >= Simulation.WorldSize) continue;
                
                // Don't evaluate positions where there is already a creature cell
                if (faunaLayer[currX, currY] != null) continue;

                var currAttractiveness = ComputeTileAttractiveness(new SimulationPosition(currX, currY), terraLayer, floraLayer, faunaLayer);

                if (currAttractiveness > bestAttractiveness)
                {
                    bestAttractiveness = currAttractiveness;
                    bestPosition = new SimulationPosition(currX, currY);
                }

                if (currAttractiveness < worstAttractiveness)
                {
                    worstAttractiveness = currAttractiveness;
                    worstPosition = new SimulationPosition(currX, currY);
                }
            }
        }
        
        // Adjust focus position according to intensity
        int bestAttractivenessIntensity = int.Abs(bestAttractiveness);
        int worstAttractivenessIntensity = int.Abs(worstAttractiveness);

        List<(MoveIntention move, int destX, int destY)> moveIntentions = [];
        
        if (bestAttractivenessIntensity > worstAttractivenessIntensity)
        {
            // Move towards best position
            if (x < bestPosition.X && x < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x + 1, y));
            }
            if (x > bestPosition.X && x > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x - 1, y));
            }
            if (y < bestPosition.Y && y < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y + 1));
            }
            if (y > bestPosition.Y && y > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y - 1));
            }
        }
        else
        {
            // Move away from worst position
            if (x < worstPosition.X && x > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x - 1, y));
            }
            if (x > worstPosition.X && x < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x + 1, y));
            }
            if (y < worstPosition.Y && y > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y - 1));
            }
            if (y > worstPosition.Y && y < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y + 1));
            }
        }

        if (moveIntentions.Count == 0) return;

        var (move, destX, destY) = moveIntentions[Simulation.RandomIntBetween(0, moveIntentions.Count)];
        
        intentionLayer[destX, destY].AddMoveIntention(move);
    }

    private int SheepMoveQuantity()
    {
        if (Quantity < 5) return Quantity;
        return 5;
    }

    public override void ComputeEatIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        EatIntentionCell[,] intentionLayer
    )
    {
        PlantCell plantCell = floraLayer[x, y];
        CreatureCell creatureCell = faunaLayer[x, y];
        
        if(creatureCell == null) return;

        if (creatureCell.Type == CreatureCellType.Sheep)
        {
            if (plantCell == null)
            {
                creatureCell.Quantity -= Constants.HUNGER_DAMAGE;
                if (creatureCell.Quantity <= 0)
                {
                    faunaLayer[x, y] = null;
                }
            }
            else
            {
                plantCell.Health -= 1 + Mathf.RoundToInt((float) creatureCell.Quantity / creatureCell.MaxQuantity);
                if (plantCell.Health <= 0)
                {
                    floraLayer[x, y] = null;
                }
            }
        }
        else if (creatureCell.Type == CreatureCellType.Wolf)
        {
            var i = new SimulationPosition(x, y);
            var availableFoodAt = new List<SimulationPosition>();
            i.ForEachDirectionBreaking(p =>
            {
                if (p.X < 0 || p.X >= faunaLayer.GetLength(0)
                         || p.Y < 0 || p.Y >= faunaLayer.GetLength(0))
                    return false;
                var c = faunaLayer[p.X, p.Y];
                if (c != null && c.Type == CreatureCellType.Sheep) availableFoodAt.Add(i);
                return false;
            });

            if (availableFoodAt.Count == 0)
            {
                // No Food :(
                creatureCell.Quantity -= Constants.HUNGER_DAMAGE;
                if (creatureCell.Quantity <= 0) faunaLayer[x, y] = null;
            }
            else
            {
                // Food :)
                var chosenFoodAt = availableFoodAt[GD.RandRange(0, availableFoodAt.Count - 1)];
                var chosenFood = faunaLayer[chosenFoodAt.X, chosenFoodAt.Y];

                chosenFood.Quantity -= Constants.FEEDING_DAMAGE;
                if (chosenFood.Quantity <= 0) faunaLayer[chosenFoodAt.X, chosenFoodAt.Y] = null;
            }
        }
        else throw new Exception("NEW CREATURE TYPE DOES NOT HAVE EAT INTENTIONS HANDLED");
    }

    public override void ComputeProcreateIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        ProcreateIntentionCell[,] intentionLayer
    )
    {
        var creatureCell = faunaLayer[x, y];
        if (creatureCell == null) return;
        creatureCell.Quantity += Constants.FERTILITY;
        if (creatureCell.Quantity > creatureCell.MaxQuantity) creatureCell.Quantity = creatureCell.MaxQuantity;
    }

    

    protected abstract int ComputeTileAttractiveness(SimulationPosition at,
        TerrainType[,] terra,
        PlantCell[,] flora,
        CreatureCell[,] fauna);
}