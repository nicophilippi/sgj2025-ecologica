using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim.creature;

public class EmptyCell() : CreatureCell(CreatureCellType.Empty, 0, 0, 0)
{
    protected override int ComputeTileAttractiveness(TerrainType terrainType, PlantCell plantCell, CreatureCell creatureCell)
    {
        return 0;
    }
}