using NewGameProject.sim.util;

namespace NewGameProject.sim.intention;

public class MoveIntention(SimulationPosition fromPosition, int quantity)
{
    public SimulationPosition FromPosition { get; } = fromPosition;
    public int Quantity { get; } = quantity;
}