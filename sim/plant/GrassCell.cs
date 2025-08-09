using NewGameProject.sim.terrain;

namespace NewGameProject.sim.plant;

public class GrassCell() : PlantCell(PlantCellType.Grass, 5)
{
    protected override bool IsHabitable(TerrainType terrainCell)
    {
        return TerrainTypeUtil.Humidity(terrainCell) >= 10 && TerrainTypeUtil.Temperature(terrainCell) is >= 0 and <= 40;
    }
}