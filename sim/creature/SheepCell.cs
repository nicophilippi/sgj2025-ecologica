using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim.creature;

public class SheepCell(int quantity) : CreatureCell(CreatureCellType.Sheep, quantity, 10, 1)
{
    protected override int ComputeTileAttractiveness(SimulationPosition at,
        TerrainType[,] terra,
        PlantCell[,] flora,
        CreatureCell[,] fauna)
    {
        var terrainType = terra[at.X, at.Y];
        var plantCell = flora[at.X, at.Y];
        var creatureCell = fauna[at.X, at.Y];
        var attractiveness = 0;

        if (terrainType == TerrainType.Plains) attractiveness += 10;

        if (plantCell != null && plantCell.Type == PlantCellType.Grass)
        {
            attractiveness += 20;
            attractiveness += plantCell.Health;
        }
        
        // if (creatureCell.Type == CreatureCellType.Wolf) attractiveness -= 100;

        return attractiveness;
    }
}