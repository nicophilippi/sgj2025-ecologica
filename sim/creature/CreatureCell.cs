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
    public int Hunger { get; set; } = 0;
    public int VisionRange { get; } = visionRange;
    
    public SimulationPosition FocusPosition = new(0, 0);
    public int FocusAttractiveness = 0;
    
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
            bestAttractiveness = 0,
            worstAttractiveness = 0;

        // Remember best and worst positions in range
        for (int currX = x - VisionRange; currX <= x + VisionRange; ++currX)
        {
            for (int currY = y - VisionRange; currY <= y + VisionRange; ++currY)
            {
                if (currX == x && currY == y) continue;
                if (currX is < 0 or >= Simulation.WorldSize || currY is < 0 or >= Simulation.WorldSize) continue;
                if (faunaLayer[currX, currY] != null && faunaLayer[currX, currY] != null) continue;

                var currAttractiveness = ComputeTileAttractiveness(new SimulationPosition(currX, currY),
                    terraLayer, floraLayer, faunaLayer);

                if (currAttractiveness > bestAttractiveness || (currAttractiveness == bestAttractiveness && Simulation.RandomIntBetween(0, 100) < 50))
                {
                    bestAttractiveness = currAttractiveness;
                    bestPosition = new SimulationPosition(currX, currY);
                }

                if (currAttractiveness < worstAttractiveness || (currAttractiveness == bestAttractiveness && Simulation.RandomIntBetween(0, 100) < 50))
                {
                    worstAttractiveness = currAttractiveness;
                    worstPosition = new SimulationPosition(currX, currY);
                }
            }
        }
        
        // Adjust focus position according to intensity
        int bestAttractivenessIntensity = int.Abs(bestAttractiveness);
        int worstAttractivenessIntensity = int.Abs(worstAttractiveness);
        
        if (bestAttractivenessIntensity > int.Abs(FocusAttractiveness))
        {
            FocusPosition = bestPosition;
            FocusAttractiveness = bestAttractiveness;
        }
        if (worstAttractivenessIntensity > int.Abs(FocusAttractiveness))
        {
            FocusPosition = worstPosition;
            FocusAttractiveness = worstAttractiveness;
        }

        List<(MoveIntention move, int destX, int destY)> moveIntentions = new();
        
        if (FocusAttractiveness > 0)
        {
            // Move towards focus position
            if (x < FocusPosition.X && x < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x + 1, y));
            }
            if (x > FocusPosition.X && x > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x - 1, y));
            }
            if (y < FocusPosition.Y && y < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y + 1));
            }
            if (y > FocusPosition.Y && y > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y - 1));
            }
        }
        else
        {
            // Move away from target position
            if (x < FocusPosition.X && x > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x - 1, y));
            }
            if (x > FocusPosition.X && x < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x + 1, y));
            }
            if (y < FocusPosition.Y && y > 0)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y - 1));
            }
            if (y > FocusPosition.Y && y < Simulation.WorldSize - 1)
            {
                moveIntentions.Add((new MoveIntention(myPosition, SheepMoveQuantity()), x, y + 1));
            }
        }

        if (moveIntentions.Count == 0) return;

        int moveIntentionIndex = Simulation.RandomIntBetween(0, moveIntentions.Count);
        
        (MoveIntention move, int destX, int destY) = moveIntentions[moveIntentionIndex];
        
        intentionLayer[destX, destY].AddMoveIntention(move);
        
        GD.Print("Moving");
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
        var plantCell = floraLayer[x, y];
        var creatureCell = faunaLayer[x, y];

        if (plantCell != null)
        {
            if (creatureCell.Quantity < creatureCell.MaxQuantity)
            {
                GD.Print(creatureCell.Quantity);
                creatureCell.Quantity++;
            }
        }
        
    }

    

    protected abstract int ComputeTileAttractiveness(SimulationPosition at,
        TerrainType[,] terra,
        PlantCell[,] flora,
        CreatureCell[,] fauna);
}