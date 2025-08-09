using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.creature;

public class SheepCell(int quantity) : CreatureCell(CreatureCellType.Sheep, quantity, 2)
{
    protected override int ComputeTileAttractiveness(TerrainType terrainType, PlantCell plantCell, CreatureCell creatureCell)
    {
        int attractiveness = 0;

        if (terrainType == TerrainType.Plains) attractiveness += 10;

        if (plantCell.Type == PlantCellType.Grass) attractiveness += 20;
        
        // if (creatureCell.Type == CreatureCellType.Wolf) attractiveness -= 100;

        return attractiveness;
    }
}