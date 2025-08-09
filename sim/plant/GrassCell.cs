using NewGameProject.sim.terrain;

namespace NewGameProject.sim.plant;

public class GrassCell() : PlantCell(PlantCellType.Grass, MAX_HEALTH)
{
    public static int MAX_HEALTH = 5;

    protected override bool IsHabitable(TerrainType terrainCell)
    {
        return TerrainTypeUtil.Humidity(terrainCell) >= 10 && TerrainTypeUtil.Temperature(terrainCell) is >= 0 and <= 40;
    }
}