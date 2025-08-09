namespace NewGameProject.sim.terrain;

public static class TerrainTypeUtil
{
    public static int Temperature(TerrainType terrainType) => terrainType switch
    {
        TerrainType.Plains => 20,
        TerrainType.Hill => 18,
        TerrainType.Mountain => 5,
        TerrainType.Desert => 40,
        TerrainType.Water => 15,
        _ => 0
    };

    public static int Humidity(TerrainType terrainType) => terrainType switch
    {
        TerrainType.Plains => 60,
        TerrainType.Hill => 50,
        TerrainType.Mountain => 50,
        TerrainType.Desert => 5,
        TerrainType.Water => 90,
        _ => 0
    };

    public static int Height(TerrainType terrainType) => terrainType switch
    {
        TerrainType.Plains => 45,
        TerrainType.Hill => 70,
        TerrainType.Mountain => 100,
        TerrainType.Desert => 30,
        TerrainType.Water => 0,
        _ => 0
    };
}