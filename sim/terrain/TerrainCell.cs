namespace NewGameProject.sim.terrain;

public abstract class TerrainCell(int height, int temperature) : Cell
{
    private int Height { get; set; } = height;
    private int Temperature { get; set; } = temperature;
}