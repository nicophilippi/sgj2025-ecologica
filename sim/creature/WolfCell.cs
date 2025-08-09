using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.creature;

public class WolfCell(int quantity) : CreatureCell(CreatureCellType.Wolf, quantity, 5, 1)
{
    protected override int ComputeTileAttractiveness(SimulationPosition at,
        TerrainType[,] terra,
        PlantCell[,] flora,
        CreatureCell[,] fauna)
    {
        var attractiveness = 0;

        at.ForEachDirectionBreaking(v =>
        {
            if (v.X < 0 || v.X >= fauna.GetLength(0)
                        || v.Y < 0 || v.Y >= fauna.GetLength(0))
                return false;
            var creature = fauna[v.X, v.Y];
            if (creature.Type == CreatureCellType.Sheep) attractiveness += creature.Quantity;
            return false;
        });

        // Wolf might stay idle when attractiveness everywhere is 0. Make him run randomly instead!
        if (attractiveness == 0) attractiveness = -1000;

        return attractiveness;
    }
}