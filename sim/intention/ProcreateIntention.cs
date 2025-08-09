using NewGameProject.sim.util;

namespace NewGameProject.sim.intention;

public class ProcreateIntention(SimulationLayer layer, SimulationPosition fromPosition, int quantity)
{
    public SimulationLayer Layer { get; } = layer;
    public SimulationPosition FromPosition { get; } = fromPosition;
    public int Quantity { get; } = quantity;
}