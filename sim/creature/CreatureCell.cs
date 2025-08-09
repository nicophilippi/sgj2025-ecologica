using System.Collections.Generic;
using NewGameProject.sim.intention;
using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.creature;

public abstract class CreatureCell(CreatureCellType type, int quantity, int visionRange) : Cell
{
    public CreatureCellType Type { get; } = type;
    
    public int Quantity { get; set; } = quantity;
    public int Hunger { get; set; } = 0;
    public int VisionRange { get; } = visionRange;
    
    public SimulationPosition FocusPosition;
    public int FocusAttractiveness;
    
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
                if (currX is < 0 or >= Simulation.WorldSize || currY is < 0 or >= Simulation.WorldSize) continue;
                
                int currAttractiveness = ComputeTileAttractiveness(terraLayer[currX, currY], floraLayer[currX, currY], faunaLayer[currX, currY]);

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

        if (FocusAttractiveness > 0)
        {
            // Move towards focus position
            if (x < FocusPosition.X)
            {
                intentionLayer[x + 1, y].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (x > FocusPosition.X)
            {
                intentionLayer[x - 1, y].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (y < FocusPosition.Y)
            {
                intentionLayer[x, y + 1].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (y > FocusPosition.Y)
            {
                intentionLayer[x, y - 1].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
        }
        else
        {
            // Move away from target position
            if (x < FocusPosition.X)
            {
                intentionLayer[x - 1, y].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (x > FocusPosition.X)
            {
                intentionLayer[x + 1, y].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (y < FocusPosition.Y)
            {
                intentionLayer[x, y - 1].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
            else if (y > FocusPosition.Y)
            {
                intentionLayer[x, y + 1].AddMoveIntention(new MoveIntention(myPosition, 1));
            }
        }
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
        
    }

    

    protected abstract int ComputeTileAttractiveness(TerrainType terrainType, PlantCell plantCell, CreatureCell creatureCell);
}