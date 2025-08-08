namespace NewGameProject.sim.creature;

public struct Creature(CreatureType type, int quantity, int hunger)
{
    private CreatureType Type { get; set; } = type;
    private int Quantity { get; set; } = quantity;
    private int Hunger { get; set; } = hunger;
}