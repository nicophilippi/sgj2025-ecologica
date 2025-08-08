using NewGameProject.sim.util;

namespace NewGameProject.sim.intention;

public class MoveIntention : Intention
{
    private SimulationPosition FromPosition { get; set; }
    private SimulationDirection Direction { get; set; }
    private int Quantity { get; set; }
}