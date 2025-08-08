namespace NewGameProject.sim.plant;

public struct Plant(PlantType type, int health)
{
    private PlantType Type { get; set; } = type;
    private int Health { get; set; } = health;
}