using NewGameProject.sim.terrain;

namespace NewGameProject.sim.plant;

public class BarrenCell() : PlantCell(PlantCellType.Barren, 0)
{
    protected override bool IsHabitable(TerrainType terrainType) => false;
}