namespace NewGameProject.sim.terrain;

public struct Terrain(int height, int temperature, TerrainType type)
{
    private TerrainType Type { get; set; } = type;
    private int Height { get; set; } = height;
    private int Temperature { get; set; } = temperature;
}