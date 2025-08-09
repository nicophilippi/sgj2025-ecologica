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
        int attractiveness = 0;
        
        // if (creatureCell.Type == CreatureCellType.Wolf) attractiveness -= 100;

        return attractiveness;
    }
}