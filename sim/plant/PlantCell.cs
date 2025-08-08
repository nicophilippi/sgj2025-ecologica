namespace NewGameProject.sim.plant;

public abstract class PlantCell(int health) : Cell
{
    private int Health { get; set; } = health;
}