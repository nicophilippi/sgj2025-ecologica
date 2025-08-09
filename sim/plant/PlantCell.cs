using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.intention.cell;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.plant;

public abstract class PlantCell(PlantCellType type, int maxHealth) : Cell
{
    public PlantCellType Type { get; } = type;
    public int Health { get; set; } = 1;
    public int MaxHealth { get; } = maxHealth;

    public sealed override void ComputeMoveIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        MoveIntentionCell[,] intentionLayer
    )
    {
        // Plants do not move...
    }

    public sealed override void ComputeEatIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        EatIntentionCell[,] intentionLayer
    )
    {
        // Grass do not eat anything...
    }

    public sealed override void ComputeProcreateIntentions(
        int x,
        int y,
        TerrainType[,] terraLayer,
        PlantCell[,] floraLayer,
        CreatureCell[,] faunaLayer,
        ProcreateIntentionCell[,] intentionLayer
    )
    {
        var currPosition = new SimulationPosition(x, y);
        var currCell = terraLayer[x, y];

        Health = IsHabitable(currCell)
            ? int.Max(Health - 1, 0)
            : int.Min(Health + 1, MaxHealth);

        if (Health == 0)
        {
            floraLayer[currPosition.X, currPosition.Y] = new BarrenCell();
        }

        if (Health < MaxHealth)
        {
            return;
        }

        currPosition.ForEachDirectionBreaking(neighborPosition =>
        {
            var neighborCell = terraLayer[neighborPosition.X, neighborPosition.Y];

            if (!IsHabitable(neighborCell)) return false;
            
            intentionLayer[neighborPosition.X, neighborPosition.Y].AddProcreateIntention(new ProcreateIntention(
                currPosition,
                1
            ));

            return true;
        });
    }



    protected abstract bool IsHabitable(TerrainType terrainType);
}