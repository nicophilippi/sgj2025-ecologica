namespace NewGameProject.sim.creature;

public abstract class CreatureCell(int quantity, int hunger) : Cell
{
    private int Quantity { get; set; } = quantity;
    private int Hunger { get; set; } = hunger;
}