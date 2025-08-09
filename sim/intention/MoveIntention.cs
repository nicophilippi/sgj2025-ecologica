using NewGameProject.sim.util;

namespace NewGameProject.sim.intention;

public class MoveIntention(SimulationPosition fromPosition, int quantity)
{
    private SimulationPosition FromPosition { get; set; } = fromPosition;
    private int Quantity { get; set; } = quantity;
}